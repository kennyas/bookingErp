using System;

namespace Booking.Payment.HttpAggregator.core.Models
{
    public class BookingCreateResponseModel
    {
        public string RefCode { get; internal set; }
        public Guid Id { get; internal set; }
        public decimal AmountPayble { get; internal set; }
        public string PaymentStatus { get; internal set; }
        public string BookingStatus { get; internal set; }
    }
}