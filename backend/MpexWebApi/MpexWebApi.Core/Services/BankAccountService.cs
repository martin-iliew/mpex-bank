using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.Services
{
    public class BankAccountService : IBankAccountService
    {
        public Task<BankAccount> CreateBankAccountAsync(string userId, int accountPlan)
        {
            throw new NotImplementedException();
        }

        public Task<Card> CreateCardAsync(string bankAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Deposit(string bankAccountId, string userId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DisableBankAccount(string userId, string bankAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FreezeCard(string cardId)
        {
            throw new NotImplementedException();
        }

        public Task TransferBetweenOwnAccounts(string bankAccountId, string senderId, string receiverIBAN, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task TransferToIBAN(string bankAccountId, string senderId, string receiverIBAN, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Withdraw(string bankAccountId, string userId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
