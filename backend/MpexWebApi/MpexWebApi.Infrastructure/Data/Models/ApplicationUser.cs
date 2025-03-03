using Microsoft.AspNetCore.Identity;
using MpexTestApi.Infrastructure.Constants.Enums;
using MpexWebApi.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexTestApi.Infrastructure.Data.Models
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }

        public AccountStatus AccountStatus { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual UserProfile? UserProfile { get; set; }

        public virtual IEnumerable<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();

        //public string? RefreshToken { get; set; }

        //public DateTime? RefreshTokenExpiry { get; set; }
    }
}
