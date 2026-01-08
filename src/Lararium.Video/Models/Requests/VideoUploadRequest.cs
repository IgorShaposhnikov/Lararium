using Lararium.Media.Module;
using Microsoft.AspNetCore.Http;
using System.Net.Quic;

namespace Lararium.Video.Models.Requests
{
    public class VideoUploadRequest
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public IFormFile FormFile { get; set; }
        public IReadOnlyCollection<Actor> Actors { get; set; } = [];
        public IReadOnlyCollection<MediaTag> MediaTags { get; set; } = [];
    }
}
