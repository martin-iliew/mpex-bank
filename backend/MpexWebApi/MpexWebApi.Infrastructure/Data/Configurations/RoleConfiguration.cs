using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static MpexWebApi.Infrastructure.Constants.EntityValidations.Admin;
using static MpexWebApi.Infrastructure.Constants.EntityValidations.ApplicationUser;
namespace MpexWebApi.Infrastructure.Data.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(SeedRoles());
        }
        private IEnumerable<IdentityRole<Guid>> SeedRoles()
        {
            IEnumerable<IdentityRole<Guid>> roles = new List<IdentityRole<Guid>>()
        {
            new IdentityRole<Guid>()
            {
                Id = Guid.NewGuid(),
                Name = UserRoleName,
                NormalizedName = UserRoleName.ToUpper()
            },
            new IdentityRole<Guid>()
            {
                Id = Guid.NewGuid(),
                Name = AdminRoleName,
                NormalizedName = AdminRoleName.ToUpper()
            }

        };

            return roles;
        }
    }
}
