using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.ViewModels
{
    public class CustomerWalletViewModel
    {
        // public Guid WalletId { get; set; } 
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public string TransactionDesc { get; set; }


    }

    public class CreditWalletViewModel
    {
        // public Guid WalletId { get; set; } 
        public string CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentReference { get; set; }
        public string TransactionDesc { get; set; }
    }

    public class CreateWalletViewModel
    {
        public Guid UserId { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class WalletTransferViewModel
    {
        public Guid CustomerId { get; set; }
        public Guid DebitWalletId { get; set; }
        public Guid CreditWalletId { get; set; }
        public decimal Amount { get; set; }
        //  public string PaymentReference { get; set; } 
        public string TransactionDesc { get; set; }
        public string Reference { get; set; }
    }


}
