using MpexWebApi.Core.ViewModels.Admin;

namespace MpexWebApi.Core.Services.Contracts
{
    public interface IAdminService
    {
        Task<List<AdminUserDto>> GetAllUsersAsync();
        Task<AdminUserDto> GetUserByIdAsync(string userId);
        Task<bool> BlockUserAsync(string userId);
        Task<bool> UnblockUserAsync(string userId);
        Task<List<AdminTransactionDto>> GetAllTransactionsAsync();
        Task<AdminTransactionDto> GetTransactionByIdAsync(string transactionId);
        Task<bool> ReverseTransactionAsync(string transactionId, string reason);
        Task<List<SuspiciousActivityDto>> GetSuspiciousActivitiesAsync();
        Task<DailySummaryReportDto> GetDailySummaryReportAsync();
        Task<bool> DisableAccountAsync(string accountId);
        Task<bool> EnableAccountAsync(string accountId);
    }
}
