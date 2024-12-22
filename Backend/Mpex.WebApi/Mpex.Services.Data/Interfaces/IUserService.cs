using Mpex.WebApi.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mpex.Services.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileViewModel> GetUserProfileInformation(string userId);
    }
}
