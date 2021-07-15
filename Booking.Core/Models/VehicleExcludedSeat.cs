using System;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class VehicleExcludedSeat : BaseEntity
    {
        public VehicleModel VehicleModel { get; set; }
        public Guid VehicleModelId { get; set; }
        public int SeatNumber { get; set; }
        public bool IsActive { get; set; }
    }
}