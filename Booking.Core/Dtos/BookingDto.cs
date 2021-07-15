using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models;

namespace Booking.Core.Dtos
{
    public class BookingDto : BaseEntity
    {
        public int BookingStatus { get; set; }



        public int ScheduleStatus { get; set; }


        public bool IsScheduled { get; set; }
        public string ScheduledUser { get; set; }
    }
}
