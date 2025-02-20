using CarApp.Infrastructure.Data.Repositories.Interfaces;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Infrastructure.Constants.Enums;
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
        private readonly IRepository<BankAccount, Guid> bankAccountRepository;
        public BankAccountService(IRepository<BankAccount, Guid> bankAccountRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
        }

        public async Task<BankAccount?> CreateBankAccountAsync(string userId, int accountPlan, int accountType)
        {
            if(!Guid.TryParse(userId, out Guid userIdGuid))
            {
                return null;
            }

            var newBankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = userIdGuid,
                AccountPlan = (AccountPlans)accountPlan,
                Balance = 0m,
                CreatedAt = DateTime.UtcNow,
                AccountType = (AccountTypes)accountType,
                IBAN = "qweqweqweqweqw"
            };

            await bankAccountRepository.AddAsync(newBankAccount);
            return newBankAccount;
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

        public Task<bool> TransferBetweenOwnAccounts(string senderAccountId, string receiverAccountId, string userId, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> TransferToIBAN(string bankAccountId, string senderId, string receiverIBAN, decimal amount)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Withdraw(string bankAccountId, string userId, decimal amount)
        {
            throw new NotImplementedException();
        }
    }
}
