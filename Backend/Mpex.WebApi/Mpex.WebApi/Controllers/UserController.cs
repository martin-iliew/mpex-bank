using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mpex.Data.Models;
using Mpex.Services;
using Mpex.Services.Interfaces;
using Mpex.WebApi.ViewModels.User;
using Mpex.WebAPI.Infrastructure.Extensions;
using System.Security.Claims;

namespace Mpex.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService userService;

        public UserController(UserManager<AppUser> userManager, IUserService userService)
        {
            _userManager = userManager;
            this.userService = userService; 
        }

        [HttpGet()]
        [Route("profile")]
        public async Task<IActionResult> GetUserProfile()
        {
            try
            {
                string userId = User.GetId();
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User is not authenticated." });
                }

                UserProfileViewModel model = await userService.GetUserProfileInformation(userId);
                if (model == null)
                {       
                    return NotFound("User profile not found.");
                }

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
