using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexTestApi.Infrastructure.Constants
{
    public static class EntityValidations
    {
        public static class ApplicationUser
        {
        }
        public static class Admin
        {
            public const string AdminRoleName = "Administrator";
            public const string AdminAreaName = "Admin";
            public const string DevelopmentAdminEmail = "admin@mpex.com";
        }
    }
}
