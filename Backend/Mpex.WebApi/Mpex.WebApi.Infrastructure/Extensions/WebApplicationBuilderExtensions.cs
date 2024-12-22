using Mpex.Data.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Cryptography;
using static Mpex.Common.EntityValidations.Admin;

namespace Mpex.WebApi.Infrastructure.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static async Task<IApplicationBuilder> SeedAdministrator(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            await EnsureRoleAsync(roleManager, "Administrator");

            var adminUser = await userManager.FindByEmailAsync("admin@mpex.com");

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    Id = Guid.Parse("30F0662A-29C5-48DF-8B57-9C61C671E0FB"),
                    UserName = (AdminAreaName),
                    NormalizedUserName = (NormalizedAreaName),
                    Email = (DevelopmentAdminEmail),
                    NormalizedEmail = (NormalizedDevelopmentAdminEmail),
                    FirstName = "Martin",
                    LastName = "Iliev",
                    ImageUrl = "https://www.shutterstock.com/image-vector/user-icon-trendy-flat-style-600nw-418179865.jpg",
                };

                var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

                if (string.IsNullOrEmpty(adminPassword))
                {
                    adminPassword = GenerateSecurePassword();
                }

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, (AdminRoleName));
                }
            }
            else
            {
                await AddRoleIfNotExist(userManager, adminUser, (AdminRoleName));
            }

            return app;
        }

        private static async Task EnsureRoleAsync(RoleManager<IdentityRole<Guid>> roleManager, string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                var result = await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Error creating '{roleName}' role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        private static async Task AddRoleIfNotExist(UserManager<AppUser> userManager, AppUser user, string roleName)
        {
            var roles = await userManager.GetRolesAsync(user);
            if (!roles.Contains(roleName))
            {
                var result = await userManager.AddToRoleAsync(user, roleName);
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Error adding user to '{roleName}' role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }

        public static string GenerateSecurePassword(int length = 16)
        {
            var randomBytes = new byte[length];
            RandomNumberGenerator.Fill(randomBytes); 
            return Convert.ToBase64String(randomBytes);
        }
    }
}
