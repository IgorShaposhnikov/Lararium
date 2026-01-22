namespace Lararium.Video.Models.Options
{
    /// <summary>
    /// Configuration options specific to AWS S3 storage for videos.
    /// </summary>
    public class VideoS3Options
    {
        /// <summary>
        /// Name of the S3 bucket where videos will be stored.
        /// </summary>
        public string BucketName { get; init; } = string.Empty;

        /// <summary>
        /// AWS region where the bucket is located (e.g., "us-east-1").
        /// </summary>
        public string Region { get; init; } = string.Empty;

        /// <summary>
        /// AWS Access Key used for authentication. Should be securely stored (e.g., in Secrets Manager).
        /// </summary>
        public string AccessKey { get; init; } = string.Empty;

        /// <summary>
        /// AWS Secret Key used for authentication. Should be securely stored (e.g., in Secrets Manager).
        /// </summary>
        public string SecretKey { get; init; } = string.Empty;

        /// <summary>
        /// Optional prefix or folder inside the bucket where videos will be stored.
        /// Allows organizing videos in a specific path within the bucket.
        /// </summary>
        public string Prefix { get; init; } = string.Empty;
    }
}
