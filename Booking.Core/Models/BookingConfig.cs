using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.Enums;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class BookingConfig : BaseEntity
    {
        public string Name { get; set; }

        public string Value { get; set; }

        public ConfigDataTypes DataType { get; set; }

        public bool IsCollection { get; set; }
    }
}
