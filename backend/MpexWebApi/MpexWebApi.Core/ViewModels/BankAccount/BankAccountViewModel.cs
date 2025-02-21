using MpexWebApi.Infrastructure.Constants.Enums;
using MpexWebApi.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.ViewModels.BankAccount
{
    public class BankAccountViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AccountType { get; set; }
        public string IBAN { get; set; }
        public decimal Balance { get; set; }
        //public virtual IEnumerable<Card> Cards { get; set; } = new List<Card>();
    }
}
