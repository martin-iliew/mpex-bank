using MpexWebApi.Core.ViewModels.BankAccount;
using MpexWebApi.Core.ViewModels.Card;
using MpexWebApi.Core.ViewModels.Cards;
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
        Task CreateBankAccountAsync(Guid userId, int accountPlan, int accountType);
        Task<bool> CreateCardAsync (Guid bankAccountId);
        Task<DebitCardViewModel?> GetCardAsync(Guid cardId);
        Task<IEnumerable<AllCardsViewModel?>> GetAllCardsAsync(Guid userId); 
        Task<bool> FreezeCard(Guid cardId);
        Task<bool> Deposit(Guid bankAccountId, decimal amount);
        Task<bool> WithdrawAsync(Guid bankAccountId, decimal amount);
        Task<bool> TransferToIBAN(Guid bankAccountId, Guid senderId, string receiverIBAN, decimal amount);
        Task<bool> TransferBetweenOwnAccounts(Guid senderAccountId, string receiverIBAN, decimal amount);
        Task<bool> DisableBankAccount(Guid userId, Guid bankAccountId);
    }
}
