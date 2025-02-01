using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MpexTestApi.Infrastructure.Data.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MpexTestApi.Core.Services.Contracts;

namespace MpexTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthController(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            IUserService userService, 
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userService = userService;
            _configuration = configuration;
        }

        //[HttpPost]
        //[Route("register")]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    var user = new ApplicationUser
        //    {
        //        UserName = model.Email,
        //        Email = model.Email,
        //        FirstName = model.FirstName,
        //        LastName = model.LastName,
        //        ImageUrl = model.ImageUrl
        //    };
        //    var result = await _userManager.CreateAsync(user, model.Password);
        //    if (result.Succeeded)
        //    {
        //        return Ok("User was registered successfully!");
        //    }
        //    return BadRequest(result.Errors);
        //}



        //[HttpPost]
        //[Route("login")]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    var user = await _userManager.FindByEmailAsync(model.Email);
        //    if (user == null)
        //    {
        //        return Unauthorized("Invalid credentials!");
        //    }

        //    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);
        //    if (!result.Succeeded)
        //    {
        //        return Unauthorized("Invalid credentials!");
        //    }

        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(ClaimTypes.Email, user.Email),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        //    };

        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    foreach (var role in userRoles)
        //    {
        //        authClaims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    var accessToken = GenerateJwtToken(authClaims);
        //    var refreshToken = GenerateRefreshToken();

        //    // Set the refresh token in an HttpOnly cookie
        //    Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Secure = true,
        //        SameSite = SameSiteMode.Strict,
        //        Expires = DateTime.UtcNow.AddDays(7)
        //    });

        //    return Ok(new
        //    {
        //        tokenType = "Bearer",
        //        accessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
        //        expiresIn = (int)(accessToken.ValidTo - DateTime.UtcNow).TotalSeconds
        //    });
        //}

        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized("No refresh token provided.");
            }
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var newAccessToken = GenerateJwtToken(claims);
            return Ok(new
            {
                tokenType = "Bearer",
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                expiresIn = (int)(newAccessToken.ValidTo - DateTime.UtcNow).TotalSeconds
            });
        }

        private JwtSecurityToken GenerateJwtToken(IEnumerable<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

            return new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddMinutes(20),
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
