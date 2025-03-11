using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MpexTestApi.Core.Services.Contracts;
using MpexTestApi.Extensions;
using MpexTestApi.Infrastructure.Data.Models;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels;

namespace MpexTestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IUserService userService;
        private readonly IBankAccountService bankAccountService;

        public AuthController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService,
            IConfiguration configuration,
            IBankAccountService bankAccountService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            this.userService = userService;
            _configuration = configuration;
            this.bankAccountService = bankAccountService;
        }

        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Register(RegisterInputModel model)
        {
            var errors = await userService.Register(model);

            if (errors.Any())
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }

            return Ok("User was registered successfully!");

        }

        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login([FromBody] LoginInputModel model)
        {
            var authResponse = await userService.Login(model);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            CreateCookie("refreshToken", authResponse.RefreshToken, 15); 

            return Ok(new { Token = authResponse.Token });
        }



        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized();
            }

            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.RefreshTokens
                .Any(r => r.RefreshTokenString == refreshToken
                && r.ExpireDate.HasValue && 
                r.ExpireDate.Value >= DateTime.UtcNow && !r.IsUsed));

            if(user == null)
            {
                return Unauthorized();
            }

            var authResponse = await userService.VerifyRefreshToken(refreshToken);

            if (authResponse == null)
            {
                return Unauthorized();
            }

            CreateCookie("refreshToken", authResponse.RefreshToken, 15);

            return Ok(new { Token = authResponse.Token });
        }

        [HttpPost]
        [Route("logout")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult LogOut()
        {
            if (Request.Cookies["refreshToken"] != null)
            {
                Response.Cookies.Delete("refreshToken");
            }

            return Ok();
        }


        private void CreateCookie(string Value, string Key, int expiryMinutes)
        {
            Response.Cookies.Append(Value, Key, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
        }
    }
}