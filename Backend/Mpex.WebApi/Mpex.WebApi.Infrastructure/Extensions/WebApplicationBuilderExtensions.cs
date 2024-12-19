using Boardtschek.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using static Boardtschek.Common.EntityValidations.GeneralApplicationConstants;

namespace Boardtschek.WebAPI.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedAdministrator(this IApplicationBuilder app, string email)
        {
            using IServiceScope scopedServices = app.ApplicationServices.CreateScope();

            IServiceProvider serviceProvider = scopedServices.ServiceProvider;

            UserManager<AppUser> userManager =
                serviceProvider.GetRequiredService<UserManager<AppUser>>();
            RoleManager<IdentityRole<Guid>> roleManager =
                serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            Task.Run(async () =>
            {
                if (await roleManager.RoleExistsAsync(AdminRoleName))
                {
                    return;
                }

                IdentityRole<Guid> role =
                    new IdentityRole<Guid>(AdminRoleName);

                await roleManager.CreateAsync(role);

                AppUser adminUser =
                    await userManager.FindByEmailAsync(email);

                await userManager.AddToRoleAsync(adminUser, AdminRoleName);
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }
    }
}
