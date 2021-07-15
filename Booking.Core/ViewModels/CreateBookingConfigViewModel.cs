using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Enums;

namespace Booking.Core.ViewModels
{
    public class CreateBookingConfigViewModel
    {
        public string Value { get; set; }
        public string Name { get; set; }

        public bool IsCollection { get; set; }
        public ConfigDataTypes DataType { get; set; }
    }


    public class EditBookingConfigRequestViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }

        public bool IsCollection { get; set; }
        public ConfigDataTypes DataType { get; set; }

    }

    public class EditBookingConfigViewModel
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }

        public bool IsCollection { get; set; }
        public ConfigDataTypes DataType { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid CreatedBy { get; set; }

    }
}
