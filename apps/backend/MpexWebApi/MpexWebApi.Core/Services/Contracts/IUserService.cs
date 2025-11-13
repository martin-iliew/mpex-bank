using Microsoft.AspNetCore.Identity;
using MpexWebApi.Infrastructure.Data.Models;
using MpexWebApi.Core.ViewModels;

namespace MpexWebApi.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<IdentityError>> Register(RegisterInputModel model);
        Task<AuthResponseViewModel?> Login(LoginInputModel model);
        Task<string> CreateRefreshToken(ApplicationUser user);
        Task<AuthResponseViewModel?> VerifyRefreshToken(string? refreshToken);

    }
}
