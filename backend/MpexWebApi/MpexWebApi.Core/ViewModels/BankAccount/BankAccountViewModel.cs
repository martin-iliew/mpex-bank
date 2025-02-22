
namespace MpexWebApi.Core.ViewModels.BankAccount
{
    using Infrastructure.Data.Models;
    using MpexWebApi.Core.ViewModels.Card;

    public class BankAccountViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AccountType { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public List<DebitCardViewModel> Cards { get; set; } = new List<DebitCardViewModel>();
    }
}
