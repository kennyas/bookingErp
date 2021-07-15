using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Area : BaseEntity
    {
        [MaxLength(100)]
        public string Title { get; set; }
        [MaxLength(300)]

        public string Description { get; set; }
        [MaxLength(10)]

        public string AreaCode { get; set; }
        public Guid StateId { get; set; }

        public State State { get; set; }
    }
}