using System;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class SubRouteFee : BaseEntity
    {
        public Guid DeparturePointId { get; set; }
        public RoutePoint DeparturePoint { get; set; }
        public decimal Fare { get; set; }

        public VehicleModel VehicleModel { get; set; }
        public Guid VehicleModelId { get; set; }
    }
}