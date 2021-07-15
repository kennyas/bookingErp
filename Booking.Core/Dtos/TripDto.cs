using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace Booking.Core.Dtos
{
    public class TripDto : BaseDto
    {
        public string Title { get; set; }
        public string DepartureTime { get; set; }
        public Guid RouteId { get; set; } //Is this code section meant to be here or under TripManagement Dto?
        public Guid VehicleId { get; set; }

        public string TripDaysDescription { get; set; }
        //public string ShortDescription { get; set; }

        public decimal? ChildrenFee { get; set; }
        public decimal? Discount { get; set; }

        public DateTime? DiscountStartDate { get; set; }

        public DateTime? DiscountEndDate { get; set; }

        public bool CanBeScheduled { get; set; }
        public int? NoOfTrips { get; set; }

        public Guid Route { get; set; }

        public string RouteName { get; set; }

        public Guid TripDaysId { get; set; }

        public string TripDaysTitle { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }







    }
}
