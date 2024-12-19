using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using static Mpex.Common.EntityValidations.AppUser;

namespace Mpex.Data.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(FirstNameMaxLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [MaxLength(LastNameMaxLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [MaxLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;
    }
}
