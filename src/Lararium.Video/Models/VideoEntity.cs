using Lararium.Media.Module;

namespace Lararium.Video.Models
{
    public class VideoEntity : MediaEntity
    {
        public string Title { get; set; }
        public string Summary { get; set; }

        public IReadOnlyCollection<MediaTag> Tags { get; set; }
        public IReadOnlyCollection<Actor> Actors { get; set; }
    }
}
