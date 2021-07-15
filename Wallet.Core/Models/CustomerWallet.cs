using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.Models
{
    public class CustomerWallet : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public decimal LedgerBalance { get; set; }
        public decimal AvailableBalance { get; set; }
        public decimal BlockedBalance { get; set; }
        public string PhoneNumber { get; set; }
        public string Fullname { get; set; }


    }

    public class CustomerWalletDto
    {
        public Guid CustomerId { get; set; }
        public Guid WalletId { get; set; }
        public string PhoneNumber { get; set; }
        public string Fullname { get; set; } 
    }
}
