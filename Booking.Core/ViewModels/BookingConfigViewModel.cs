using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Enums;

namespace Booking.Core.ViewModels
{
    public class BookingConfigViewModel
    {
        public Guid? Id { get; set; }
        public object Value { get; set; } 
        public string Name { get; set; }


        public Guid? CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }


        public bool IsCollection { get; set; }
        public ConfigDataTypes DataType { get; set; }
    }
}
