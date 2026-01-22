using Asp.Versioning;
using Lararium.Media.Module;
using Lararium.Persistence.Core;
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
        private readonly IDataStore<VideoEntity, Guid> _videoDataStore;
        private readonly VideoServiceOptions _options;

        public VideoController(
            IDataStore<VideoEntity, Guid> videoDataStore,
            IOptions<VideoServiceOptions> options,
            ILogger<VideoController> logger)
        {
            _videoDataStore = videoDataStore
                ?? throw new ArgumentNullException(nameof(videoDataStore));
            _options = options.Value
                ?? throw new ArgumentNullException(nameof(options));
            _logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
        }

        private async Task SaveUploadedFileAsync(IFormFile formFile, VideoEntity video, string path, CancellationToken cancellationToken)
        {
            var fileName = $"{video.Id}{video.FileExt}";
            var finalFilePath = Path.Combine(path, fileName);

            await using var targetFile = System.IO.File.OpenWrite(finalFilePath);
            await formFile.CopyToAsync(targetFile, cancellationToken);
        }

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [DisableRequestSizeLimit]
        [RequestFormLimits(MultipartBodyLengthLimit = long.MaxValue)]
        public async Task<IActionResult> Upload([FromForm] VideoUploadRequest request, CancellationToken cancellationToken)
        {
            var path = Path.GetFullPath(
                Path.Combine(_options.LocalStorageRoot, _options.VideosSubfolder)
                );

            if (string.IsNullOrWhiteSpace(path))
            {
                return BadRequest("Video storage is not configured. Contact administrator.");
            }

            var driveInfo = new DriveInfo(Path.GetPathRoot(path)!);
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

            await SaveUploadedFileAsync(request.FormFile, mediaInfo, path, cancellationToken);

            await _videoDataStore.AddAsync(mediaInfo, cancellationToken);
            await _videoDataStore.SaveChangesAsync(cancellationToken);

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

        [HttpGet("video/{videoId}")]
        public async Task<IActionResult> GetVideo(string videoId, CancellationToken cancellationToken)
        {
            if (!Guid.TryParse(videoId, out var id))
            {
                return NotFound();
            }

            var entity = await _videoDataStore.GetAsync(id, cancellationToken);

            var path = Path.GetFullPath(
                Path.Combine(_options.LocalStorageRoot, _options.VideosSubfolder)
            );

            var filePath = Path.Combine(path, $"{entity.Id}{entity.FileExt}");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            if (Path.GetExtension(path) == ".m3u8")
            {
                filePath = Path.Combine(path, "master.m3u8");
                return Content(filePath, "application/vnd.apple.mpegurl");
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            // Set content type and enable range processing
            return File(fileStream, "video/mp4", enableRangeProcessing: true);
        }
    }
}