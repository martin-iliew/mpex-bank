using System.Security.Claims;
using static MpexWebApi.Infrastructure.Constants.EntityValidations.Admin;

namespace MpexWebApi.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string? GetId(this ClaimsPrincipal user)
        {
            return user?.FindFirstValue("uid");
        }
        public static bool isAdmin(this ClaimsPrincipal user)
        {
            return user.IsInRole(AdminRoleName);
        }


    }
}
