using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class TripManagement : BaseEntity
    {
        public Guid? DriverId { get; set; }
        public Captain Driver { get; set; }
        public BusBoy BusBoy { get; set; }
        public Guid? BusBoyId { get; set; }
        public TripManagementStatus Status { get; set; }
        public DateTime DepartureDate { get; set; }
        public Guid TripId { get; set; }
        [ForeignKey(nameof(TripId))]
        public Trip Trip { get; set; }
    }
}