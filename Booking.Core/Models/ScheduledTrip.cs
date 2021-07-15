using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class ScheduledTrip : BaseEntity
    {
        public Guid TripId { get; set; }
        [ForeignKey(nameof(TripId))]
        public Trip Trip { get; set; }
        public Guid CustomerId { get; set; }
        public bool CanHaveReturn { get; set; }
        //have a start date and end date
        public DateTime ScheduleStartDate { get; set; }
        public DateTime ScheduleEndDate { get; set; }
        //run this schedule every week
        //public Duration Duration { get; set; }
        public string ReturnTime { get; set; }
        public ScheduleStatus ScheduleStatus { get; set; }
        public int? PreferredSeat { get; set; }
    }
}
