using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class UpdateBookingByExternalRefViewModel
    {
        public int SeatNumber { get; set; }
        [Required]
        public string RefCode { get; set; }
        [Required]

        public string GIGRefCode { get; set; }
        [Required]
        public string Channel { get; set; }


    }
}
