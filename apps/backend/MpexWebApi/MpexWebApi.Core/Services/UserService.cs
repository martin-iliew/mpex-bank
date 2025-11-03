using CarApp.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Infrastructure.Constants.Enums;
using MpexWebApi.Infrastructure.Data;
using MpexWebApi.Infrastructure.Data.Models;
using MpexWebApi.Core.Services;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels;
using MpexWebApi.Infrastructure.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static MpexWebApi.Infrastructure.Constants.EntityValidations.ApplicationUser;
namespace MpexWebApi.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private readonly IBankAccountService bankAccountService;
        private readonly AppDbContext context;
        private readonly IRepository<RefreshToken, Guid> refreshTokenRepository;

        private const string loginProvider = "MpexApi";
        private const string refreshToken = "RefreshToken";

        public UserService(UserManager<ApplicationUser> userManager,
            IConfiguration config, IBankAccountService bankAccountService,
            AppDbContext context, IRepository<RefreshToken, Guid> refreshTokenRepository)
        {
            this.userManager = userManager;
            this.config = config;
            this.bankAccountService = bankAccountService;
            this.context = context;
            this.refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<IEnumerable<IdentityError>> Register(RegisterInputModel model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                return new List<IdentityError>
                {
                    new IdentityError {
                        Code = "PasswordMismatch",
                        Description = "Password and Confirm Password do not match." }
                };
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                AccountStatus = AccountStatus.Pending,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserProfile = new UserProfile
                {
                    Id = Guid.NewGuid(),
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = new DateOnly(2004, 2, 5)
                }

            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, UserRoleName);
                await bankAccountService.CreateBankAccountAsync(user.Id, model.AccountPlan, 0);
            }


            return result.Errors;
            
        }

        public async Task<AuthResponseViewModel?> Login(LoginInputModel model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                return null;
            }
            var token = await GenerateToken(user);

            return new AuthResponseViewModel
            {
                Token = token,
                UserId = user.Id.ToString(),
                RefreshToken = await CreateRefreshToken(user)
            };

        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim("role", role)).ToList();
            var userClaims = await userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim("uid", user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: config["JwtSettings:Issuer"],
                audience: config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(config["JwtSettings:DurationInMinutes"]!)),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken(ApplicationUser user)
        {
            string newRefreshToken = await userManager.GenerateUserTokenAsync(user, loginProvider,
                refreshToken);

            var expiryDate = DateTime.UtcNow.AddMinutes(15);

            var refreshTokenModel = new RefreshToken
            {
                Id = Guid.NewGuid(),
                RefreshTokenString = newRefreshToken,
                ExpireDate = expiryDate,
                UserId = user.Id,
                IsUsed = false
            };


            await refreshTokenRepository.AddAsync(refreshTokenModel);

            return newRefreshToken;
        }

        public async Task<AuthResponseViewModel?> VerifyRefreshToken(string? refreshToken)
        {
            var refreshTokenRecord = await refreshTokenRepository
                .FirstOrDefaultAsync(r => r.RefreshTokenString == refreshToken && !r.IsUsed
                && r.ExpireDate.HasValue && r.ExpireDate.Value >= DateTime.UtcNow);

            if (refreshTokenRecord == null)
            {
                return null; 
            }
            refreshTokenRecord.IsUsed = true;

            await refreshTokenRepository.UpdateAsync(refreshTokenRecord);

            var user = await userManager.FindByIdAsync(refreshTokenRecord.UserId.ToString());

            if(user == null)
            {
                return null;
            }

            var newAccessToken = await GenerateToken(user);
            var newRefreshToken = await CreateRefreshToken(user);

            return new AuthResponseViewModel
            {
                UserId = user.Id.ToString(),
                Token = newAccessToken,
                RefreshToken = newRefreshToken

            };
        }
    }
}
