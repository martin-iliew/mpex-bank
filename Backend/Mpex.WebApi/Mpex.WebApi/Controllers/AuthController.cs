using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Mpex.Data.Models;
using Mpex.Services.Interfaces;
using Mpex.WebApi.ViewModels.User;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Mpex.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService userService;

        public AuthController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, IUserService userService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.userService = userService;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ImageUrl = model.ImageUrl
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return Ok("User was registered successfully!");
            }
            return BadRequest(result.Errors);
        }


      
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Unauthorized("Invalid credentials!");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
            if (!result.Succeeded)
            {
                return Unauthorized("Invalid credentials!");
            }

            // Generate JWT Token
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Add user roles to claims
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Generate access token
            var accessToken = GenerateJwtToken(authClaims);

            // Generate refresh token
            var refreshToken = GenerateRefreshToken();

            // Set the refresh token in an HttpOnly cookie
            Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true,    // Ensures the cookie cannot be accessed via JavaScript
                Secure = true,      // Ensures the cookie is only sent over HTTPS
                SameSite = SameSiteMode.Strict,  // Protects against CSRF attacks
                Expires = DateTime.UtcNow.AddDays(7) // Set the expiry time for the refresh token cookie
            });

            return Ok(new
            {
                tokenType = "Bearer",
                accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                expiresIn = (int)(accessToken.ValidTo - DateTime.UtcNow).TotalSeconds
            });
        }

        private JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
