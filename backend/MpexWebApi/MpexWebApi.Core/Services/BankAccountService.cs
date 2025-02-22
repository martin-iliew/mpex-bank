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
using System.Numerics;
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
                AccountNumber = GenerateAccountNumber(),
                AccountPlan = (AccountPlans)accountPlan,
                Balance = 0m,
                CreatedAt = DateTime.UtcNow,
                AccountType = (AccountTypes)accountType
            };

            newBankAccount.IBAN = GenerateIBAN(newBankAccount.AccountNumber);

            await bankAccountRepository.AddAsync(newBankAccount);
            return newBankAccount;
        }

        public Task<Card?> CreateCardAsync(string bankAccountId)
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


        public string GenerateAccountNumber()
        {
            Random random = new Random();
            string accountNumber;

            do
            {
                // Generate a random 12-digit number
                accountNumber = random.NextInt64(100000000000, 999999999999).ToString("D12");
            } while (bankAccountRepository.GetAllAttached()
            .Any(account => account.AccountNumber == accountNumber));

            return accountNumber;
        }

        public string GenerateIBAN(string accountNumber)
        {
            const string countryCode = "BG";
            const string bankCode = "MPEX";

            // Construct the initial IBAN structure: BG + check digits + bank code + account number
            string ibanWithoutCheckDigits = countryCode + "00" + bankCode + accountNumber;

            // Calculate the check digits for the IBAN
            string ibanForCheck = ibanWithoutCheckDigits + "131400"; // '131400' represents the numeric value of "BG"
            int checkDigits = Mod97(ibanForCheck);

            // Replace the placeholder check digits with the calculated check digits
            string iban = countryCode + checkDigits.ToString("D2") + bankCode + accountNumber;

            return iban;
        }

        // Mod97 algorithm to calculate IBAN check digits
        private int Mod97(string iban)
        {
            // Convert the string to a numeric value
            string ibanAsNumeric = string.Concat(iban
                .Select(c => char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString()));

            // Perform Mod97 on the numeric value
            BigInteger numericValue = BigInteger.Parse(ibanAsNumeric);
            return (int)(numericValue % 97);
        }
    }
}
