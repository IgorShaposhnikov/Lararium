using Lararium.Media.Models;
using Lararium.Media.Module;
using System.ComponentModel.DataAnnotations;

namespace Lararium.Video.Models
{
    /// <summary>
    /// Represents a video entity in the system, extending the base <see cref="MediaEntity"/> class.
    /// This class contains video-specific metadata and relationships with tags and actors.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This entity is typically persisted to a database and used throughout the video processing
    /// and playback system. It inherits common media properties from <see cref="MediaEntity"/>.
    /// </para>
    /// <para>
    /// The entity supports many-to-many relationships with <see cref="Actor"/> and <see cref="MediaTag"/>
    /// entities through the respective collection properties.
    /// </para>
    /// </remarks>
    public class VideoEntity : MediaEntity
    {
        /// <summary>
        /// Gets or sets the display title of the video.
        /// </summary>
        /// <value>
        /// The title of the video. Should be unique within the system for better SEO and user experience.
        /// </value>
        [Required(ErrorMessage = "Video title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        [Display(Name = "Video Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a brief description or summary of the video content.
        /// </summary>
        /// <value>
        /// A summary providing context about the video content, suitable for display in listings and previews.
        /// </value>
        [Display(Name = "Description")]
        public string Summary { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the collection of tags associated with this video.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="MediaTag"/> entities used for categorization and search.
        /// </value>
        /// <remarks>
        /// Tags help with content discovery, filtering, and organization within the video library.
        /// </remarks>
        public IReadOnlyCollection<MediaTag> Tags { get; set; } = [];

        /// <summary>
        /// Gets or sets the collection of actors appearing in this video.
        /// </summary>
        /// <value>
        /// A read-only collection of <see cref="Actor"/> entities representing the performers in the video.
        /// </value>
        /// <remarks>
        /// Actors are linked for search functionality, credits display, and content filtering.
        /// </remarks>
        public IReadOnlyCollection<Actor> Actors { get; set; } = [];
    }
}
