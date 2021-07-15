using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.Models
{
    public class WalletHistory : BaseEntity
    {
        [MaxLength(150)]
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public Guid WalletId { get; set; }   
        [MaxLength(150)]
        public string Currency { get; set; }
        [MaxLength(150)]
        public string TransType { get; set; } 
        [MaxLength(300)]
        public string TransactionDesc { get; set; }
        [MaxLength(300)]
        public string PaymentReference { get; set; }
    }
}
