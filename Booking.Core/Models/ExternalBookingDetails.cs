using Booking.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class GigBookingDetails : BaseEntity
    {
        [MaxLength(50)]
        public string Channel { get; set; }
        [MaxLength(100)]
        public string GIGRefCode { get; set; }
        [MaxLength(20)]
        public string CustomerPhoneNumber { get; set; }
        [MaxLength(200)]
        public string CustomerEmail { get; set; }

        public GIGRefCodeStatus GIGRefCodeStatus { get; set; }

    }
}
