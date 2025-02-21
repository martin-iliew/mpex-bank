using CarApp.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Infrastructure.Constants.Enums;
using MpexWebApi.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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

        public async Task<BankAccountViewModel?> GetBankAccountAsync(Guid bankAccountId)
        {
            var model = await bankAccountRepository
                .GetAllAttached()
                .Where(ba => ba.Id.ToString() == bankAccountId.ToString())
                .Select(ba => new BankAccountViewModel()
                {
                    Id = ba.Id.ToString(),
                    UserId = ba.UserId.ToString(),
                    IBAN = ba.IBAN,
                    Balance = ba.Balance,
                    AccountType = Enum.GetName(ba.AccountType),
                    //Cards = ba.Cards
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return model;
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
