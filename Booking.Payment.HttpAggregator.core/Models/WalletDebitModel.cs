using System;

namespace Booking.Payment.HttpAggregator.core.Models
{
    public class WalletDebitModel
    {
        public Guid CustomerId { get; set; }
        public decimal Amount { get; set; }
        public string Reference { get; set; }
        public string TransactionDesc { get; set; }
    }
}