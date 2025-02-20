using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MpexTestApi.Core.Services.Contracts;
using MpexTestApi.Infrastructure.Constants.Enums;
using MpexTestApi.Infrastructure.Data.Models;
using MpexWebApi.Core.Services;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static MpexTestApi.Infrastructure.Constants.EntityValidations.ApplicationUser;
namespace MpexTestApi.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private ApplicationUser user;
        private readonly IBankAccountService bankAccountService;

        private const string loginProvider = "MpexApi";
        private const string refreshToken = "RefreshToken";

        public UserService(UserManager<ApplicationUser> userManager,
            IConfiguration config, IBankAccountService bankAccountService)
        {
            this.userManager = userManager;
            this.config = config;
            this.bankAccountService = bankAccountService;
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

            var user = new ApplicationUser
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

            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, UserRoleName);
            }
            //var createBankAccount = await bankAccountService
            //    .CreateBankAccountAsync(user.Id.ToString(), 0);

            return result.Errors;
            
        }

        public async Task<AuthResponseViewModel> Login(LoginInputModel model)
        {
            user = await userManager.FindByEmailAsync(model.Email);
            bool isvalidUser = await userManager.CheckPasswordAsync(user, model.Password);
            if (user == null || isvalidUser == false)
            {
                return null;
            }
            var token = await GenerateToken();

            return new AuthResponseViewModel
            {
                Token = token,
                UserId = user.Id.ToString(),
                RefreshToken = await CreateRefreshToken()
            };

        }

        public async Task<string> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(config["JwtSettings:Key"]!));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(role => new Claim(ClaimTypes.Role, role)).ToList();
            var userClaims = await userManager.GetClaimsAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id.ToString()),
            }
            .Union(userClaims).Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: config["JwtSettings:Issuer"],
                audience: config["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(config["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string> CreateRefreshToken()
        {
            await userManager.RemoveAuthenticationTokenAsync(user, loginProvider, refreshToken);

            var newRefreshToken = await userManager.GenerateUserTokenAsync(user, loginProvider,
                refreshToken);
            var result = await userManager.SetAuthenticationTokenAsync(user, loginProvider,
                refreshToken, newRefreshToken);

            return newRefreshToken;
        }

        public async Task<AuthResponseViewModel> VerifyRefreshToken(AuthResponseViewModel request)
        {
            var jwtSecurityTokenHanfler = new JwtSecurityTokenHandler();
            var tokenContent = jwtSecurityTokenHanfler.ReadJwtToken(request.Token);
            var username = tokenContent.Claims.ToList().FirstOrDefault(q => q.Type ==
            JwtRegisteredClaimNames.Email)?.Value;
            user = await userManager.FindByNameAsync(username);

            if(user == null || user.Id.ToString() != request.UserId)
            {
                return null;
            }
            var isValidRefreshToken = await userManager.VerifyUserTokenAsync(user, loginProvider,
                refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var token = await GenerateToken();
                return new AuthResponseViewModel
                {
                    Token = token,
                    UserId = user.Id.ToString(),
                    RefreshToken = await CreateRefreshToken()
                };
            }

            await userManager.UpdateSecurityStampAsync(user);
            return null;
        }
    }
}
