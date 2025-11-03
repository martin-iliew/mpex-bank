using MpexWebApi.Infrastructure.Data.Models;
using MpexWebApi.Infrastructure.Constants.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MpexWebApi.Infrastructure.Data.Models
{
    public class BankAccount
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
        public string AccountNumber { get; set; }
        public AccountPlans AccountPlan { get; set; }
        public string IBAN { get; set; }
        
        public AccountTypes AccountType { get; set; }
        
        public decimal Balance { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? DisabledAt { get; set; }

        public virtual IEnumerable<Card> Cards { get; set; } = new List<Card>();

    }
}
