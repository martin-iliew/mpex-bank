﻿using Microsoft.AspNetCore.Identity;
using MpexTestApi.Infrastructure.Constants.Enums;
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
        public string PhoneNmber { get; set; } = null!;

        public AccountStatus AccountStatus { get; set; }

        public string? ProfilePicture { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public virtual UserProfile? UserProfile { get; set; }
    }
}
