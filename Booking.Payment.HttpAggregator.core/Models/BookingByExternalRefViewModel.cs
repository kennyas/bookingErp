using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Payment.HttpAggregator.core.Models
{
    public class BookingByExternalRefViewModel
    {
        public int SeatNumber { get; set; }
        public string RefCode { get; set; }
        public string GIGRefCode { get; set; }
    }
}
