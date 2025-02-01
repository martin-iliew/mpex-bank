using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MpexTestApi.Infrastructure.Data.Models
{
    public class UserProfile
    {
        [Key]
        public Guid Id { get; set; }

        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public DateOnly DateOfBirth { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public virtual ApplicationUser?  User { get; set; }


    }
}
