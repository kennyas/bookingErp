using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Trip : BaseEntity
    {
        public Trip()
        {
        }

        public string Title { get; set; }

        public TimeSpan DepartureTime { get; set; }
        //depending on the customers age
        //if null... no discount
        public decimal? Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
        public bool CanBeScheduled { get; set; }
        public Guid? VehicleId { get; set; }
        [ForeignKey(nameof(VehicleId))]
        public Vehicle Vehicle { get; set; }
        public DateTime? LastTripCreationDate { get; set; }
        public bool IsActive { get; set; }

        public Guid RouteId { get; set; }
        public Route Route { get; set; }

        public Guid TripDaysId { get; set; }
        public TripDays TripDays { get; set; }
    }
}