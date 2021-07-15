using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Wallet.Core.Dtos
{

    public class WalletDebitDto : BaseEntity 
    {
        public Guid CustomerId { get; set; }
        public Guid DebitWalletId { get; set; }
        public string Reference { get; set; } 
        public decimal Amount { get; set; }
        public string PaymentReference { get; set; }
        public string TransType { get; set; }
        public string TransactionDesc { get; set; }
        public decimal NewBalance { get; set; }
    }

    public class WalletTransferDto : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Guid DebitWalletId { get; set; }
        public Guid CreditWalletId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentReference { get; set; }
        public string TransType { get; set; }
        public string TransactionDesc { get; set; }
        public decimal NewBalance { get; set; }
        public string Reference { get; set; }
    }

    public class WalletTransHistory : BaseEntity
    {
        public string Reference { get; set; }
        public decimal Amount { get; set; }
        public Guid WalletId { get; set; }
        public string TransactionDesc { get; set; }
        public string TransType { get; set; }

    }

    //public class WalletHistoryViewModel 
    //{
    //    public string CustomerId { get; set; }
    //    public string StartDate { get; set; } 
    //    public string EndDate { get; set; } 
    //}

    public class WalletHistoryViewModel
    {
        public WalletHistoryViewModel() 
        {
            PageIndex = 0;
            PageTotal = 10;
        }
        [JsonProperty("CustomerId")]
        public string CustomerId { get; set; }
        [JsonProperty("StartDate")]
        public string StartDate { get; set; }
        [JsonProperty("EndDate")]
        public string EndDate { get; set; }
        [JsonProperty("pageIndex")]
        public int? PageIndex { get; set; }
        [JsonProperty("pageTotal")]
        public int? PageTotal { get; set; }
    }
}
