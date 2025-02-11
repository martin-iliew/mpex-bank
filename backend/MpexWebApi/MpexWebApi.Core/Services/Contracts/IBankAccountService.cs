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
        Task<BankAccount> CreateBankAccountAsync(string userId, int accountPlan);

        Task<Card> CreateCardAsync(string bankAccountId);

        Task<bool> FreezeCard(string cardId);

        Task<bool> Deposit(string bankAccountId, string userId, decimal amount);

        Task<bool> Withdraw(string bankAccountId, string userId, decimal amount);

        Task TransferToIBAN(string bankAccountId, string senderId, string receiverIBAN, decimal amount);

        Task TransferBetweenOwnAccounts(string bankAccountId, string senderId, string receiverIBAN, decimal amount);
        
        Task<bool> DisableBankAccount(string userId, string bankAccountId);
    }
}
