using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.Models
{
    public class CardDetails : BaseEntity
    {
        [MaxLength(4)]

        public string LastFourDigits { get; set; }

        public Guid CustomerId { get; set; }

        public string CardType { get; set; }
        public string AuthCode { get; set; }
    }
}
