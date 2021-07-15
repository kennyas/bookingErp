using System.ComponentModel.DataAnnotations;

namespace Booking.Payment.HttpAggregator.core.Models
{
    public class PayModelRequest
    {
        [Required]
        public string Refcode { get; set; }

        public string GIGRefCode { get; set; }

        public string Channel { get; set; }

        public bool IsGigPay { get; set; }
    }
}