using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.ViewModels
{
    public class LoginInputModel
    {
        [Required]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(6, MinimumLength = 4)]
        public string Password { get; set; } = null!;
    }
}
