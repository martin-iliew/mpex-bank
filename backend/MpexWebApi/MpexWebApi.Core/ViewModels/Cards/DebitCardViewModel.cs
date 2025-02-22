using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Core.ViewModels.Card
{
    public class DebitCardViewModel
    {
        public string Id { get; set; }
        public string UserId { get; set; }

        public string CardNumber { get; set; }

        public string CVV { get; set; }

        public string ExpiaryDate { get; set; }

        public string OwnerName { get; set; }

        public string CardStatus { get; set; }
    }
}
