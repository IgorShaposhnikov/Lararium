using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lararium.Media.Models
{
    public class MediaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public long FileSize { get; init; }
        public string FileType { get; init; } = string.Empty;
        public string FileExt { get; init; } = string.Empty;
    }
}
