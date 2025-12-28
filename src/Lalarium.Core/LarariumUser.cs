
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lararium.Core
{
    public class LarariumUser : IUser
    {
        public LarariumUser()
        {
            Id = Guid.CreateVersion7();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Login { get; set; }
        public string? PasswordHash { get; set; }
    }
}
