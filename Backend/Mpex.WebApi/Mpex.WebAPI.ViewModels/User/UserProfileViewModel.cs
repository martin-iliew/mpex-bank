using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpex.WebApi.ViewModels.User
{
    public class UserProfileViewModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
    }
}
