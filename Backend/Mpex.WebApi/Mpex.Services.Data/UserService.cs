using Microsoft.EntityFrameworkCore;
using Mpex.Data;
using Mpex.Data.Models;
using Mpex.Services.Interfaces;
using Mpex.WebApi.ViewModels.User;

namespace Mpex.Services
{
    public class UserService : IUserService
    {
        private readonly MpexDbContext dbContext;
        public UserService(MpexDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<UserProfileViewModel> GetUserProfileInformation(string userId)
        {
            AppUser user = await dbContext.Users.FirstAsync(u => u.Id.ToString() == userId);
            UserProfileViewModel model = new()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                ImageUrl = user.ImageUrl
            };
            return model;
        }
    }
}
