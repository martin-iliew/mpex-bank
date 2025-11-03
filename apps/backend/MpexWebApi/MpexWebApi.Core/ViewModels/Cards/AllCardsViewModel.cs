using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.ViewModels.Cards
{
    public class AllCardsViewModel
    {
        public string Id { get; set; }

        public string BankAccountId { get; set; }
        public string CardNumber { get; set; }

        public decimal Balance { get; set; }

        public string CVV { get; set; }

        public string ExpiaryDate { get; set; }

        public string OwnerName { get; set; }

        public string CardStatus { get; set; }

        public string AccountPlan { get; set; }

        public string AccountType { get; set; }
    }
}
