using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mpex.Data.Models;

namespace Mpex.Data
{
    public class MpexDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public MpexDbContext(DbContextOptions<MpexDbContext> options) : base(options)
        {

        }
    }
}
