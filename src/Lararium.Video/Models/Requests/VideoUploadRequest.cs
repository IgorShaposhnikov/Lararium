using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Lararium.Video.Models.Requests
{
    /// <summary>
    /// Represents a request for uploading a video file along with associated metadata.
    /// This class is designed to be used with multipart/form-data HTTP requests.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All properties use init-only setters to ensure immutability after creation.
    /// </para>
    /// <para>
    /// Collections are initialized as empty read-only collections to prevent null reference exceptions.
    /// </para>
    /// </remarks>
    public class VideoUploadRequest
    {
        /// <summary>
        /// Gets the title of the video.
        /// </summary>
        /// <value>A string representing the video's display title. Required field.</value>
        [Required(ErrorMessage = "Video title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets a brief summary or description of the video content.
        /// </summary>
        /// <value>A string describing the video content. Optional field, may be empty or null.</value>
        [StringLength(1000, ErrorMessage = "Summary cannot exceed 1000 characters.")]
        public string? Summary { get; set; } = string.Empty;

        /// <summary>
        /// Gets the video file to be uploaded.
        /// </summary>
        /// <value>An <see cref="IFormFile"/> representing the uploaded video file. Required field.</value>
        [Required(ErrorMessage = "Video file is required.")]
        public IFormFile FormFile { get; set; } = default!;

        /// <summary>
        /// Gets the collection of actors associated with this video.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="Guid"/> values representing actor IDs.
        /// Optional field, may be null or empty.
        /// </value>
        public IReadOnlyCollection<Guid>? Actors { get; init; } = [];

        /// <summary>
        /// Gets the collection of tags associated with this video.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="Guid"/> values representing media tag IDs.
        /// Optional field, may be null or empty.
        /// </value>
        public IReadOnlyCollection<Guid>? MediaTags { get; init; } = [];
    }
}
