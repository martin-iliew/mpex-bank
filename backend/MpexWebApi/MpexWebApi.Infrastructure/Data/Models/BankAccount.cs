using MpexTestApi.Infrastructure.Data.Models;
using MpexWebApi.Infrastructure.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Infrastructure.Data.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public AccountPlans AccountPlan { get; set; }
        public string IBAN { get; set; }
        
        public AccountTypes AccountType { get; set; }
        
        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual IEnumerable<Card> Cards { get; set; } = new List<Card>();

    }
}
