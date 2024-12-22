using System.ComponentModel.DataAnnotations;
using static Mpex.Common.EntityValidations.AppUser;

namespace Mpex.WebApi.ViewModels.User
{
    public class RegisterViewModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 5 characters long.")]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(FirstNameMaxLength, MinimumLength = FirstNameMinLength)]
        public string FirstName { get; set; } = null!;

        [Required]
        [StringLength(LastNameMaxLength, MinimumLength = LastNameMinLength)]
        public string LastName { get; set; } = null!;

        [Required]
        [Url]
        [StringLength(ImageUrlMaxLength)]
        public string ImageUrl { get; set; } = null!;
    }
}
