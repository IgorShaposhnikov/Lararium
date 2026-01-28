namespace Lararium.Video.Encoders
{
    public interface IVideoEncoder<T> where T : IVideoEncoder<T>
    {
        public Task Encode(string videoPath, string outputPath, CancellationToken cancellationToken = default!);
    }
}
