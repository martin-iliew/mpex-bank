using MpexWebApi.Infrastructure.Constants.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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

        public CardStatuses CardStatus { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
