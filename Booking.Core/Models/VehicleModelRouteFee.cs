using System;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class VehicleModelRouteFee:BaseEntity
    {
        public Guid VehicleModelId { get; set; }
        public VehicleModel VehicleModel { get; set; }
        public Guid RouteId { get; set; }
        public Route Route { get; set; }
        public decimal BaseFee { get; set; }

        //other types of fee for that route
        //public decimal? DispatchFee { get; set; }
    }
}