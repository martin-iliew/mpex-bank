using MpexWebApi.Core.ViewModels.Card;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.ViewModels.BankAccount
{
    public class AllBankAccountViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AccountType { get; set; }
        public string AccountPlan { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        public int Cards { get; set; }
    }
}
