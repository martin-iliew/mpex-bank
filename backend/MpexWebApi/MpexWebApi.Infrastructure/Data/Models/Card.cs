using MpexWebApi.Infrastructure.Constants.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MpexWebApi.Infrastructure.Data.Models
{
    public class Card
    {
        public Guid Id { get; set; }

        public string CardNumber { get; set; }

        public string CVV { get; set; }

        public Guid BankAccountId { get; set; }
        [ForeignKey(nameof(BankAccountId))]
        public BankAccount BankAccount { get; set; }

        public DateOnly ExpiryDate { get; set; }

        public CardStatuses CardStatus { get; set; } = CardStatuses.Active;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
