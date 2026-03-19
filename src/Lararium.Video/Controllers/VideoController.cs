using Asp.Versioning;
using Lararium.Core.Persistence;
using Lararium.Media.Module;
using Lararium.Video.Encoders;
using Lararium.Video.Models;
using Lararium.Video.Models.Options;
using Lararium.Video.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq.Expressions;

namespace Lararium.Video.Controllers
{
    [Authorize]
    [ApiController]
    [ApiVersion(1.0)]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class VideoController : ControllerBase
    {
        private readonly ILogger<VideoController> _logger;
        private readonly IVideoEncoder<HlsEncoder> _hlsEncoder;
        private readonly IDataStore<VideoEntity, Guid> _videoDataStore;
        private readonly VideoServiceOptions _options;

        public VideoController(
            IDataStore<VideoEntity, Guid> videoDataStore,
            IOptions<VideoServiceOptions> options,
            ILogger<VideoController> logger,
            IVideoEncoder<HlsEncoder> hlsEncoder)
        {
            _videoDataStore = videoDataStore
                ?? throw new ArgumentNullException(nameof(videoDataStore));
            _options = options.Value
                ?? throw new ArgumentNullException(nameof(options));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            _hlsEncoder = hlsEncoder;
        }

        private async Task SaveUploadedFileAsync(IFormFile formFile, VideoEntity video, string uploadPath, CancellationToken cancellationToken)
        {
            var fileName = $"{video.Id}{video.FileExt}";
            var finalFilePath = Path.Combine(uploadPath, fileName);

            await using var targetFile = System.IO.File.OpenWrite(finalFilePath);
            await formFile.CopyToAsync(targetFile, cancellationToken);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> Upload([FromForm] VideoUploadRequest request, CancellationToken cancellationToken)
        {
            var uploadPath = Path.GetFullPath(
                Path.Combine(_options.LocalStorageRoot, _options.UploadSubfolder)
                );

            if (string.IsNullOrWhiteSpace(uploadPath))
            {
                return BadRequest("Video storage is not configured. Contact administrator.");
            }

            var driveInfo = new DriveInfo(Path.GetPathRoot(uploadPath)!);
            if (driveInfo.AvailableFreeSpace < request.FormFile.Length)
            {
                return BadRequest($"Not enough disk space. Available: {driveInfo.AvailableFreeSpace / (1024 * 1024)}MB");
            }

            var fileExt = Path.GetExtension(request.FormFile.FileName);

            var mediaInfo = new VideoEntity()
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                Summary = request.Summary ?? string.Empty,
                Actors = [.. request.Actors!.Select(id => new Actor { Id = id })],
                Tags = [.. request.MediaTags!.Select(id => new MediaTag { Id = id })],
                FileExt = fileExt,
                FileSize = request.FormFile.Length,
                FileType = fileExt,
            };

            await SaveUploadedFileAsync(request.FormFile, mediaInfo, uploadPath, cancellationToken);

            await _videoDataStore.AddAsync(mediaInfo, cancellationToken);
            await _videoDataStore.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        [HttpPost("upload/hls")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> UploadToHLS([FromForm] VideoUploadRequest request, CancellationToken cancellationToken)
        {
            var uploadPath = Path.GetFullPath(
                Path.Combine(_options.LocalStorageRoot, _options.UploadSubfolder)
                );

            if (string.IsNullOrWhiteSpace(uploadPath))
            {
                return BadRequest("Video storage is not configured. Contact administrator.");
            }

            var driveInfo = new DriveInfo(Path.GetPathRoot(uploadPath)!);
            if (driveInfo.AvailableFreeSpace < request.FormFile.Length)
            {
                return BadRequest($"Not enough disk space. Available: {driveInfo.AvailableFreeSpace / (1024 * 1024)}MB");
            }

            var fileExt = Path.GetExtension(request.FormFile.FileName);

            var mediaInfo = new VideoEntity()
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                Summary = request.Summary ?? string.Empty,
                Actors = [.. request.Actors!.Select(id => new Actor { Id = id })],
                Tags = [.. request.MediaTags!.Select(id => new MediaTag { Id = id })],
                FileExt = ".m3u8",
                FileSize = request.FormFile.Length,
                FileType = ".m3u8",
            };

            var fileName = $"{mediaInfo.Id}{fileExt}";

            if (!Directory.Exists(uploadPath)) 
            {
                Directory.CreateDirectory(uploadPath);
            }

            var finalFilePath = Path.Combine(uploadPath, fileName);

            {
                await using var targetFile = System.IO.File.OpenWrite(finalFilePath);
                await request.FormFile.CopyToAsync(targetFile, cancellationToken);
            }

            await _videoDataStore.AddAsync(mediaInfo, cancellationToken);
            await _videoDataStore.SaveChangesAsync(cancellationToken);

            var m3u8Output = Path.Combine(_options.LocalStorageRoot, _options.VideosSubfolder, mediaInfo.Id.ToString());

            await _hlsEncoder.Encode(finalFilePath, m3u8Output, cancellationToken: cancellationToken);

            return Ok();
        }

        [HttpGet("list")]
        public Task<IEnumerable<VideoEntity>> GetVideosAsync([FromQuery] VideoListRequest videoListRequest, CancellationToken cancellationToken = default)
        {
            Expression<Func<VideoEntity, bool>> whereExpression = video =>
                string.IsNullOrEmpty(videoListRequest.Search) ||
                video.Title.ToLower().Contains(videoListRequest.Search.ToLower(), StringComparison.OrdinalIgnoreCase);

            return _videoDataStore.GetEntitiesAsync(
                where: whereExpression,
                skip: videoListRequest.Skip,
                take: videoListRequest.Take,
                cancellationToken: cancellationToken);
        }

        [HttpGet("fileSizeLimit")]
        public int GetVideoFileSizeLimit()
        {
            return _options.MaxFileSizeMb;
        }

        [HttpGet("{videoId}")]
        public async Task<ActionResult<VideoEntity>> GetVideo(string videoId)
        {
            if (!Guid.TryParse(videoId, out var id))
            {
                return NotFound();
            }

            return Ok(await _videoDataStore.GetAsync(id));
        }

        [HttpGet("{videoId}/stream")]
        public async Task<IActionResult> GetVideoStreaming(string videoId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(videoId, out var id))
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(_options.LocalStorageRoot))
            {
                return BadRequest("Video storage is not configured. Contact administrator.");
            }

            var entity = await _videoDataStore.GetAsync(id, cancellationToken: cancellationToken);

            var path = Path.GetFullPath(
                Path.Combine(_options.LocalStorageRoot, _options.VideosSubfolder)
            );

            var filePath = string.Empty;

            if (entity.FileExt == ".m3u8")
            {
                filePath = Path.Combine(path, entity.Id.ToString(), $"master.m3u8");
            }
            else
            {
                filePath = Path.Combine(path, $"{entity.Id}{entity.FileExt}");
            }

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if (Path.GetExtension(filePath) == ".m3u8")
            {
                Response.Headers.Add("Access-Control-Allow-Origin", "*");
                Response.Headers.Add("Cache-Control", "no-cache");

                var contentType = "application/vnd.apple.mpegurl";

                return PhysicalFile(filePath, contentType, enableRangeProcessing: true);
            }

            var fileStream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.Read,
                bufferSize: 64 * 1024, // 64KB buffer
                useAsync: true);

            var result = new FileStreamResult(fileStream, "video/mp4")
            {
                EnableRangeProcessing = true
            };
            return result;
        }

        [AllowAnonymous]
        [HttpGet("{videoId}/segment/{segmentName}")]
        public IActionResult GetSegment(string videoId, string segmentName)
        {
            var filePath = Path.Combine(Path.Combine(_options.LocalStorageRoot, _options.VideosSubfolder), videoId.ToString(), segmentName);
            return PhysicalFile(filePath, "video/MP2T", enableRangeProcessing: true);
        }
    }
}