using CarApp.Infrastructure.Data.Repositories;
using CarApp.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.Admin;
using MpexWebApi.Infrastructure.Constants.Enums;
using MpexWebApi.Infrastructure.Data;
using MpexWebApi.Infrastructure.Data.Models;
using System.Reflection;
using System.Text;
using static MpexWebApi.Infrastructure.Constants.EntityValidations.Admin;

namespace MpexWebApi.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.RegisterRepositories(typeof(ApplicationUser).Assembly);

            services.RegisterUserDefinedServices(typeof(IUserService).Assembly);

            services.AddHttpClient();

            return services;
        }
        public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));


            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, 
                    ValidIssuer = config["JwtSettings:Issuer"],
                    ValidAudience = config["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        {
                            context.Token = authHeader.Substring("Bearer ".Length);
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }

        public static IServiceCollection AddApplicationIdentity(this IServiceCollection services, IConfiguration config)
        {

            services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 4;
            })
            .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>("MpexApi")
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();


            return services;
        }

        public static void RegisterRepositories(this IServiceCollection services, Assembly modelsAssembly)
        {
            Type[] typesToExclude = new Type[] { typeof(ApplicationUser), typeof(UserProfile), typeof(AccountPlans) };

            Type[] modelsType = modelsAssembly
                .GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface && !t.IsEnum &&
                        !t.Name.ToLower().EndsWith("attribute"))
                .ToArray();

            foreach (Type type in modelsType)
            {
                if (!typesToExclude.Contains(type))
                {
                    Type repositoryInterface = typeof(IRepository<,>);
                    Type repositoryInstanceType = typeof(BaseRepository<,>);

                    PropertyInfo? idPropInfo = type
                        .GetProperties()
                        .Where(p => p.Name.ToLower() == "id")
                        .SingleOrDefault();

                    Type[] constructArgs = new Type[2];
                    constructArgs[0] = type;

                    if (idPropInfo == null)
                    {
                        constructArgs[1] = typeof(object);
                    }
                    else
                    {
                        constructArgs[1] = idPropInfo.PropertyType;
                    }

                    repositoryInterface = repositoryInterface.MakeGenericType(constructArgs);
                    repositoryInstanceType = repositoryInstanceType.MakeGenericType(constructArgs);

                    services.AddScoped(repositoryInterface, repositoryInstanceType);
                }
            }
        }
        public static void RegisterUserDefinedServices(this IServiceCollection services, Assembly serviceAssembly)
        {
            Type[] serviceInterfaceTypes = serviceAssembly
                .GetTypes()
                .Where(t => t.IsInterface)
                .ToArray();

            Type[] serviceTypes = serviceAssembly
                .GetTypes()
                .Where(t => !t.IsInterface &&
                            !t.IsAbstract &&
                             t.Name.ToLower().EndsWith("service"))
                .ToArray();

            foreach (Type serviceInterfaceType in serviceInterfaceTypes)
            {
                Type? serviceType = serviceTypes
                    .SingleOrDefault(t => "i" + t.Name.ToLower() == serviceInterfaceType.Name.ToLower());

                if (serviceType == null)
                {
                    throw new NullReferenceException($"Service type could not be obtained for the service {serviceInterfaceType.Name}");
                }
                services.AddScoped(serviceInterfaceType, serviceType);
            }
        }

        public static async Task SeedAdminAsync(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var serviceProvider = scope.ServiceProvider;

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            var adminSettings = serviceProvider.GetRequiredService<IOptions<AdminUserSettings>>().Value;

            if (!await roleManager.RoleExistsAsync(AdminRoleName))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(AdminRoleName));
            }

            var adminUser = await userManager.FindByEmailAsync(adminSettings.Email);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = adminSettings.Email,
                    Email = adminSettings.Email,
                    EmailConfirmed = true,
                    PhoneNumber = adminSettings.PhoneNumber,
                    AccountStatus = AccountStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    UserProfile = new UserProfile
                    {
                        Id = Guid.NewGuid(),
                        FirstName = adminSettings.FirstName,
                        LastName = adminSettings.LastName,
                        DateOfBirth = adminSettings.DateOfBirth
                    }
                };

                var result = await userManager.CreateAsync(adminUser, adminSettings.Password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));
                }

                await userManager.AddToRoleAsync(adminUser, AdminRoleName);

                dbContext.UserProfiles.Add(adminUser.UserProfile!);
                await dbContext.SaveChangesAsync();
            }
        }

    }
}
