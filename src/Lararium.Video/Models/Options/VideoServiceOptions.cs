namespace Lararium.Video.Models.Options
{
    /// <summary>
    /// Configuration options for the Video Service, supporting both local and cloud storage.
    /// This class contains settings for video storage paths, processing, validation, and upload constraints.
    /// </summary>
    public class VideoServiceOptions
    {
        /// <summary>
        /// If true, videos will be stored in S3 instead of the local filesystem.
        /// </summary>
        public bool UseCloudStorage { get; init; } = false;

        /// <summary>
        /// Gets or sets the root directory for local video storage.
        /// This is the base path where all video files are stored locally.
        /// </summary>
        public string LocalStorageRoot { get; init; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the subfolder for temporary uploads within <see cref="LocalStorageRoot"/>.
        /// Uploaded videos are initially stored here before processing.
        /// </summary>
        /// <value>Default is "Temp".</value>
        public string UploadSubfolder { get; init; } = "Temp";

        /// <summary>
        /// Gets or sets the name of the subfolder for processed videos within <see cref="LocalStorageRoot"/>.
        /// After processing, videos are moved from the temporary folder to this location.
        /// </summary>
        /// <value>Default is "Processed".</value>
        public string VideosSubfolder { get; init; } = "Processed";

        /// <summary>
        /// Indicates whether uploaded videos should be transcoded after upload.
        /// If false, videos will remain in their original format.
        /// </summary>
        public bool EnableTranscoding { get; init; } = true;

        /// <summary>
        /// Maximum allowed video file size in megabytes (MB).
        /// Any uploaded video exceeding this limit should be rejected.
        /// </summary>
        public int MaxFileSizeMb { get; init; } = 1024 * 20;

        /// <summary>
        /// List of allowed file extensions for video uploads.
        /// Only files with these extensions will be accepted.
        /// </summary>
        public List<string> AllowedExtensions { get; init; } = new() { ".mp4", ".mkv", ".mov", ".avi" };

        /// <summary>
        /// Maximum number of upload attempts allowed for a single video.
        /// Helps prevent abuse and repeated failed uploads.
        /// </summary>
        public int MaxUploadAttempts { get; init; } = 5;

        /// <summary>
        /// List of allowed MIME types for video uploads.
        /// Used to validate that uploaded files match expected video content types.
        /// </summary>
        public List<string> AllowedMimeTypes { get; init; } = new()
        {
            "video/mp4",
            "video/x-matroska",
            "video/quicktime",
            "video/x-msvideo"
        };

        /// <summary>
        /// S3 storage configuration.
        /// Only used when <see cref="UseCloudStorage"/> is true.
        /// </summary>
        public VideoS3Options S3 { get; init; } = new();
    }
}
