using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Payment.HttpAggregator.core.Models
{
    public class ExternalCodeVerificationModel
    {
        public string RefCode { get; set; }
        public string ExternalRefCode { get; set; }
    }
}
