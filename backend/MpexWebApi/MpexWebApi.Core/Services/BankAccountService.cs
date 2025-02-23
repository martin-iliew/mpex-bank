using CarApp.Infrastructure.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using MpexWebApi.Core.Services.Contracts;
using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
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

        public async Task<bool> CreateCardAsync(Guid bankAccountId)
        {
            var bankAccount = await bankAccountRepository
                .FirstOrDefaultAsync(ba => ba.Id == bankAccountId);

            if(bankAccount == null)
            {
                return false;
            }

            var newCard = new Card
            {
                Id = Guid.NewGuid(),
                BankAccountId = bankAccountId,
                ExpiryDate = GenerateExpiryDate(),
                CreatedAt = DateTime.Now,
                CardStatus = 0,
                CardNumber = GenerateCardNumber(bankAccount.AccountNumber),
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
                    Cards = ba.Cards.Select(c => new DebitCardViewModel()
                    {
                        Id = c.Id.ToString(),
                        CardNumber = c.CardNumber,
                    }).ToList()
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
            int expiryYear = DateTime.Now.Year + 3;

            int expiryMonth = DateTime.Now.Month;

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
