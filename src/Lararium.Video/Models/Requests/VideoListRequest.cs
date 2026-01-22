using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Lararium.Video.Models.Requests
{
    /// <summary>
    /// Represents a request for retrieving a paginated list of videos with optional filtering and sorting.
    /// </summary>
    public class VideoListRequest
    {
        /// <summary>
        /// Gets or sets the search term for filtering videos by title or summary.
        /// </summary>
        /// <value>A search string to filter videos. If null or empty, no search filtering is applied.</value>
        public string? Search { get; set; }
        /// <summary>
        /// Gets or sets the page number for pagination (1-based).
        /// </summary>
        /// <value>The page number to retrieve. Must be greater than 0. Default is 1.</value>
        [Range(1, int.MaxValue, ErrorMessage = "Page must be greater than 0.")]
        public int Page { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of items per page.
        /// </summary>
        /// <value>The maximum number of videos to return per page. Must be between 1 and 100. Default is 20.</value>
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100.")]
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Gets the calculated number of items to skip for pagination.
        /// </summary>
        [JsonIgnore]
        [BindNever]
        internal int Skip => (Page - 1) * PageSize;
        /// <summary>
        /// Gets the calculated number of items to take for pagination.
        /// </summary>
        [JsonIgnore]
        [BindNever]
        internal int Take => PageSize;
    }
}
