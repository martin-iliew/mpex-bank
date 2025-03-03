using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
using MpexWebApi.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.Services.Contracts
{
    public interface IBankAccountService
    {
        Task<IEnumerable<AllBankAccountViewModel?>> GetAllBankAccountAsync(Guid UserId);
        Task<BankAccountViewModel?> GetBankAccountAsync(Guid bankAccountId);
        Task<BankAccount?> CreateBankAccountAsync(string userId, int accountPlan, int accountType);

        Task<bool> CreateCardAsync (Guid bankAccountId);
        Task<DebitCardViewModel?> GetCardAsync(Guid cardId);

        Task<bool> FreezeCard(string cardId);

        Task<bool> Deposit(Guid bankAccountId, decimal amount);

        Task<bool> WithdrawAsync(Guid bankAccountId, decimal amount);

        Task<bool> TransferToIBAN(string bankAccountId, string senderId, string receiverIBAN, decimal amount);

        Task<bool> TransferBetweenOwnAccounts(string senderAccountId, string receiverAccountId, string userId, decimal amount);
        
        Task<bool> DisableBankAccount(string userId, string bankAccountId);
    }
}
