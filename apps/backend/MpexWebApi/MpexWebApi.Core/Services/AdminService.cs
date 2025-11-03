using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.Admin;

namespace MpexWebApi.Core.Services
{
    public class AdminService : IAdminService
    {
        public Task<bool> BlockUserAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DisableAccountAsync(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> EnableAccountAsync(string accountId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdminTransactionDto>> GetAllTransactionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DailySummaryReportDto> GetDailySummaryReportAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<SuspiciousActivityDto>> GetSuspiciousActivitiesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<AdminTransactionDto> GetTransactionByIdAsync(string transactionId)
        {
            throw new NotImplementedException();
        }

        public Task<AdminUserDto> GetUserByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReverseTransactionAsync(string transactionId, string reason)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UnblockUserAsync(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
