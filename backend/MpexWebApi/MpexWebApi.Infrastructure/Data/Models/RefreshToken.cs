using MpexWebApi.Infrastructure.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MpexWebApi.Infrastructure.Data.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }

        public string? RefreshTokenString { get; set; }

        public DateTime? ExpireDate { get; set; }

        public bool IsUsed { get; set; }

    }
}
