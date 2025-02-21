using MpexWebApi.Core.ViewModels.BankAccount;
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
        Task<BankAccountViewModel?> GetBankAccountAsync(Guid bankAccountId);
        Task<BankAccount?> CreateBankAccountAsync(string userId, int accountPlan, int accountType);

        Task<Card?> CreateCardAsync(string bankAccountId);

        Task<bool> FreezeCard(string cardId);

        Task<bool> Deposit(string bankAccountId, string userId, decimal amount);

        Task<bool> Withdraw(string bankAccountId, string userId, decimal amount);

        Task<bool> TransferToIBAN(string bankAccountId, string senderId, string receiverIBAN, decimal amount);

        Task<bool> TransferBetweenOwnAccounts(string senderAccountId, string receiverAccountId, string userId, decimal amount);
        
        Task<bool> DisableBankAccount(string userId, string bankAccountId);
    }
}
