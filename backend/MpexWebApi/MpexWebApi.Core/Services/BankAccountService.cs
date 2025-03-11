﻿using CarApp.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
using MpexWebApi.Core.ViewModels.Cards;
using MpexWebApi.Infrastructure.Constants.Enums;
using MpexWebApi.Infrastructure.Data.Models;
using System.Globalization;
using System.Numerics;
using System.Text.RegularExpressions;

namespace MpexWebApi.Core.Services
{
    public class BankAccountService : IBankAccountService
    {
        private static Random random = new Random();
        private readonly IRepository<BankAccount, Guid> bankAccountRepository;
        private readonly IRepository<Card, Guid> cardRepository;
        public BankAccountService(IRepository<BankAccount, Guid> bankAccountRepository,
            IRepository<Card, Guid> cardRepository)
        {
            this.bankAccountRepository = bankAccountRepository;
            this.cardRepository = cardRepository;
        }

        public async Task<IEnumerable<AllBankAccountViewModel?>> GetAllBankAccountAsync(Guid UserId)
        {
            var bankAccounts = await bankAccountRepository
                .GetAllAttached()
                .Where(ba => ba.ApplicationUser.Id.Equals(UserId))
                .Select(ba => new AllBankAccountViewModel()
                {
                    Id = ba.Id.ToString(),
                    UserId = ba.UserId.ToString(),
                    IBAN = ba.IBAN,
                    Balance = ba.Balance,
                    AccountType = ba.AccountType.ToString(),
                    AccountPlan = ba.AccountPlan.ToString(),
                    Cards = ba.Cards.Count()
                })
                .ToListAsync();

            return bankAccounts;
        }

        public async Task<IEnumerable<AllCardsViewModel?>> GetAllCardsAsync(Guid userId)
        {
            var bankAccounts = await cardRepository
                .GetAllAttached()
                .Where(c => c.BankAccount.ApplicationUser.Id.Equals(userId))
                .Select(c => new AllCardsViewModel()
                {
                    Id = c.Id.ToString(),
                    BankAccountId = c.BankAccountId.ToString(),
                    Balance = c.BankAccount.Balance,
                    CardNumber = Regex.Replace(c.CardNumber, @"(\d{4})(\d{4})(\d{4})(\d{4})", "$1 $2 $3 $4"),
                    CVV = c.CVV,
                    ExpiaryDate = c.ExpiryDate.ToString("MM/yy", CultureInfo.InvariantCulture),
                    OwnerName = $"{c.BankAccount.ApplicationUser.UserProfile.FirstName} {c.BankAccount.ApplicationUser.UserProfile.LastName}",
                    CardStatus = c.CardStatus.ToString() ?? "",
                    AccountPlan = c.BankAccount.AccountPlan.ToString() ?? "",
                    AccountType = c.BankAccount.AccountType.ToString() ?? ""
                })
                .OrderBy(c => c.Balance)
                .ToListAsync();

            return bankAccounts;
        }
        public async Task CreateBankAccountAsync(Guid userId, int accountPlan, int accountType)
        {
            var newBankAccount = new BankAccount
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                AccountNumber = GenerateAccountNumber(),
                AccountPlan = (AccountPlans)accountPlan,
                Balance = 0m,
                CreatedAt = DateTime.UtcNow,
                AccountType = (AccountTypes)accountType
            };

            newBankAccount.IBAN = GenerateIBAN(newBankAccount.AccountNumber);

            await bankAccountRepository.AddAsync(newBankAccount);

            await CreateCardAsync(newBankAccount.Id);
        }

        public async Task<bool> CreateCardAsync(Guid bankAccountId)
        {
            var bankAccount = await bankAccountRepository
                .FirstOrDefaultAsync(ba => ba.Id == bankAccountId);

            if (bankAccount == null)
            {
                return false;
            }

            var newCard = new Card
            {
                Id = Guid.NewGuid(),
                BankAccountId = bankAccountId,
                ExpiryDate = GenerateExpiryDate(),
                CreatedAt = DateTime.UtcNow,
                CardStatus = 0,
                CardNumber = GenerateCardNumber(bankAccount.AccountNumber) ?? "xxxxxxxxxxxxxxxx",
                CVV = GenerateCVV()
            };
            await cardRepository.AddAsync(newCard);

            return true;
        }

        public async Task<DebitCardViewModel?> GetCardAsync(Guid cardId)
        {
            var model = await cardRepository
                .GetAllAttached()
                .Where(c => c.Id.ToString() == cardId.ToString())
                .Select(c => new DebitCardViewModel()
                {
                    Id = c.Id.ToString(),
                    UserId = c.BankAccount.UserId.ToString(),
                    CardNumber = Regex.Replace(c.CardNumber, @"(\d{4})(\d{4})(\d{4})(\d{4})", "$1 $2 $3 $4"),
                    CVV = c.CVV,
                    ExpiaryDate = c.ExpiryDate.ToString("MM/yy", CultureInfo.InvariantCulture),
                    OwnerName = $"{c.BankAccount.ApplicationUser.UserProfile.FirstName} {c.BankAccount.ApplicationUser.UserProfile.LastName}",
                    CardStatus = Enum.GetName(c.CardStatus)
                })
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> Deposit(Guid bankAccountId, decimal amount)
        {
            var bankAccount = await bankAccountRepository.GetByIdAsync(bankAccountId);
            if(bankAccount == null)
            {
                return false;
            }
            bankAccount.Balance += amount;
            await bankAccountRepository.UpdateAsync(bankAccount);
            return true;
        }

        public Task<bool> DisableBankAccount(Guid userId, Guid bankAccountId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> FreezeCard(Guid cardId)
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
                    Cards = ba.Cards.Select(c => new DebitCardViewModel()
                    {
                        Id = c.Id.ToString(),
                        UserId = c.BankAccount.UserId.ToString(),
                        CardNumber = Regex.Replace(c.CardNumber, @"(\d{4})(\d{4})(\d{4})(\d{4})", "$1 $2 $3 $4"),
                        CVV = c.CVV,
                        CardStatus = c.CardStatus.ToString(),
                        ExpiaryDate = c.ExpiryDate.ToString("MM/yy", CultureInfo.InvariantCulture),
                        OwnerName = $"{c.BankAccount.ApplicationUser.UserProfile.FirstName} {c.BankAccount.ApplicationUser.UserProfile.LastName}"
                    }).ToList()
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return model;
        }

        public async Task<bool> TransferBetweenOwnAccounts(Guid senderAccountId, string receiverIBAN, decimal amount)
        {
            var senderAccount = await bankAccountRepository
                .FirstOrDefaultAsync(ba => ba.Id == senderAccountId);

            var receiverAccount = await bankAccountRepository
                .FirstOrDefaultAsync(ba => ba.IBAN == receiverIBAN);

            if (senderAccount == null || receiverAccount == null)
            {
                return false;
            }

            if (senderAccount.UserId != receiverAccount.UserId)
            {
                return false;
            }

            if (senderAccount.IBAN == receiverAccount.IBAN)
            {
                return false;
            }

            if (senderAccount.Balance < amount || amount <= 0)
            {
                return false;
            }

            senderAccount.Balance -= amount;
            receiverAccount.Balance += amount;

            await bankAccountRepository.UpdateAsync(senderAccount);
            await bankAccountRepository.UpdateAsync(receiverAccount);

            return true;
        }

        public async Task<bool> TransferToIBAN(Guid senderBankAccountId, string receiverIBAN, decimal amount)
        {
            var senderAccount = await bankAccountRepository
                .FirstOrDefaultAsync(ba => ba.Id == senderBankAccountId);

            if (senderAccount == null)
            { 
                return false;
            }
            if (senderAccount.Balance < amount || amount <= 0)
            {
                return false;
            }

            var receiverAccount = await bankAccountRepository
                .FirstOrDefaultAsync(ba => ba.IBAN == receiverIBAN);
            if (receiverAccount != null)
            {
                senderAccount.Balance -= amount;
                receiverAccount.Balance += amount;
                await bankAccountRepository.UpdateAsync(senderAccount);
                await bankAccountRepository.UpdateAsync(receiverAccount);
            }

            return true;
        }

        public async Task<bool> WithdrawAsync(Guid bankAccountId, decimal amount)
        {
            var bankAccount = await bankAccountRepository.GetByIdAsync(bankAccountId);
            if (bankAccount == null)
            {
                return false;
            }

            if (bankAccount.Balance < amount)
            {
                return false;
            }

            bankAccount.Balance -= amount;
            await bankAccountRepository.UpdateAsync(bankAccount);

            return true;
        }


        public string GenerateAccountNumber()
        {
            string accountNumber;

            do
            {
                accountNumber = random.NextInt64(100000000, 999999999).ToString("D9");
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

        private int Mod97(string iban)
        {
            // Convert the string to a numeric value
            string ibanAsNumeric = string.Concat(iban
                .Select(c => char.IsLetter(c) ? (c - 'A' + 10).ToString() : c.ToString()));

            // Perform Mod97 on the numeric value
            BigInteger numericValue = BigInteger.Parse(ibanAsNumeric);
            return (int)(numericValue % 97);
        }

        public static DateOnly GenerateExpiryDate()
        {
            int expiryYear = DateTime.UtcNow.Year + 3;

            int expiryMonth = DateTime.UtcNow.Month;

            DateOnly expiryDate = new DateOnly(expiryYear, expiryMonth, 
                DateTime.DaysInMonth(expiryYear, expiryMonth));

            return expiryDate;
        }

        public static string GenerateCVV()
        {
            return random.Next(100, 1000).ToString();
        }

        public string? GenerateCardNumber(string bankAccountNumber)
        {
            string issuerIdentificationNumber = "411231";
            int maxSequence = 9;
            string cardNumber;

            for (int sequence = 0; sequence <= maxSequence; sequence++)
            {
                string accountIdentifier = bankAccountNumber.Substring(0, Math.Min(8, bankAccountNumber.Length));
                string sequenceNumber = sequence.ToString();
                string cardNumberWithoutChecksum = issuerIdentificationNumber + accountIdentifier + sequenceNumber;

                int checksum = CalculateLuhnChecksum(cardNumberWithoutChecksum);
                cardNumber = cardNumberWithoutChecksum + checksum;

                bool exists = cardRepository.GetAll().Any(c => c.CardNumber == cardNumber);
                if (!exists)
                {
                    return cardNumber;
                }
            }

            return null;
        }

        private static int CalculateLuhnChecksum(string cardNumberWithoutChecksum)
        {
            int sum = 0;
            bool alternate = false;
            for (int i = cardNumberWithoutChecksum.Length - 1; i >= 0; i--)
            {
                int digit = int.Parse(cardNumberWithoutChecksum[i].ToString());
                if (alternate)
                {
                    digit *= 2;
                    if (digit > 9)
                        digit -= 9;
                }
                sum += digit;
                alternate = !alternate;
            }
            int checksum = (10 - (sum % 10)) % 10;
            return checksum;
        }
    }
}
