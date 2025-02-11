using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.ViewModels
{
    public class RegisterInputModel
    {
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [RegularExpression(@"^\d{4,6}$", ErrorMessage = "Password must be a 4 to 6-digit PIN.")]

        public string Password { get; set; } = null!;
        
        [Required]
        [RegularExpression(@"^\d{4,6}$", ErrorMessage = "Password must be a 4 to 6-digit PIN.")]
        public string ConfirmPassword { get; set; } = null!;

    }
}
