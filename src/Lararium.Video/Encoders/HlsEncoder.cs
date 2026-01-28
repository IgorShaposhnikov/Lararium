using Microsoft.Extensions.Logging;

namespace Lararium.Video.Encoders
{
    public class HlsEncoder : IVideoEncoder<HlsEncoder>
    {
        private readonly ILogger<HlsEncoder> _logger;
        private readonly int _segmentDuration;

        public HlsEncoder(ILogger<HlsEncoder> logger, int segmentDuration = 6)
        {
            _logger = logger;
            _segmentDuration = segmentDuration;
        }

        public Task Encode(string videoPath, string outputPath, CancellationToken cancellationToken = default!)
        {
            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            var playlistPath = Path.Combine(outputPath, "master.m3u8");
            var segmentPath = Path.Combine(outputPath, $"segment_%03d.ts");

            return FFMpegCore.FFMpegArguments
                .FromFileInput(videoPath)
                .OutputToFile(playlistPath, true, x =>
                {
                    x.WithCustomArgument("-c:v copy");
                    x.WithCustomArgument("-c:a copy");
                    x.ForceFormat("hls").WithCustomArgument($"-hls_time {_segmentDuration}").WithCustomArgument("-hls_list_size 0");
                    x.WithCustomArgument($"-hls_base_url \"{{baseUrl}}\"");
                    x.WithCustomArgument($"-hls_segment_filename {segmentPath}");
                }).ProcessAsynchronously();
        }
    }
}
