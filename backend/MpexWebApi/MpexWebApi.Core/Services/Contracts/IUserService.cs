using Microsoft.AspNetCore.Identity;
using MpexWebApi.Core.ViewModels;

namespace MpexTestApi.Core.Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<IdentityError>> Register(RegisterInputModel model);
        Task<AuthResponseViewModel> Login(LoginInputModel model);
        Task<string> CreateRefreshToken();
        Task<AuthResponseViewModel> VerifyRefreshToken(AuthResponseViewModel request);

    }
}
