using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Partner : BaseEntity
    {
        [MaxLength(100)]
        public string FirstName { get; set; }  
        [MaxLength(100)]
        public string LastName { get; set; }
        [MaxLength(100)]
        public string Email { get; set; }
        [MaxLength(50)]
        public string PhoneNumber { get; set; }
    }
}
