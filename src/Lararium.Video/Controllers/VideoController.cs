using Asp.Versioning;
using Lararium.Persistence.Core;
using Lararium.Video.ActionFilters;
using Lararium.Video.Models;
using Lararium.Video.Models.Options;
using Lararium.Video.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;

namespace Lararium.Video.Controllers
{
    [ApiController]
    [ApiVersion(1.0)]
    [Authorize]
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

        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ServiceFilter(typeof(VideoUploadFilter))]
        public async Task<IActionResult> Upload([FromForm] VideoUploadRequest request, CancellationToken cancellationToken)
        {
            var path = Path.GetFullPath(_options.UploadTempPath);

            var fileExt = Path.GetExtension(request.FormFile.FileName);
            var fileName = $"{Guid.CreateVersion7()}{fileExt}";
            var finalFilePath = Path.Combine(path, fileName);

            await using var targetFile = System.IO.File.OpenWrite(finalFilePath);
            await request.FormFile.CopyToAsync(targetFile, cancellationToken);

            var mediaInfo = new VideoEntity()
            {
                Id = Guid.CreateVersion7(),
                Title = request.Title,
                Summary = request.Summary,
                Actors = request.Actors,
                Tags = request.MediaTags,
                FileExt = fileExt,
                FileSize = request.FormFile.Length,
                FileType = fileExt,
            };

            await _videoDataStore.AddAsync(mediaInfo, cancellationToken);
            await _videoDataStore.SaveChangesAsync(cancellationToken);

            return Ok();
        }

        const string path = "video\\HSLResult\\";

        [HttpGet("video/{filename}")]
        public IActionResult GetVideo(string fileName, IWebHostEnvironment env)
        {
            var filePath = Path.Combine(path, "master.m3u8");

            return Content(filePath, "application/vnd.apple.mpegurl");
        }


        [HttpGet("list")]
        public Task<IEnumerable<VideoEntity>> GetVideosAsync(CancellationToken cancellationToken = default) 
        {
            return _videoDataStore.GetAllAsync(cancellationToken);
        } 

        [HttpGet("segment")]
        //[AllowAnonymous]
        public IActionResult Segment(string segment)
        {
            var filePath = Path.Combine("D:\\Programming\\Lararium\\src\\Lararium.UI.Swelte\\static\\", path, segment);
            //if (!System.IO.File.Exists(filePath))
            //    return NotFound();

            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, "video/MP2T");
        }
    }
}