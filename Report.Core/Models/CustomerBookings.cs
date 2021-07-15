using System;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Report.Core.Models
{
    public class CustomerBookings : BaseEntity
    {
        public string CustomerName { get; set; }
        public string BookingStatus { get; set; }

        [MaxLength(320)]
        public string EmailAddress { get; set; }

        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        public DateTime BookingDate { get; set; }

        [MaxLength(20)]
        public string PaymentMethod { get; set; }

        [MaxLength(20)]
        public string PaymentStatus { get; set; }

        public decimal Amount { get; set; }
        public string DepatureTime { get; set; }

        public DateTime? DepartureDate { get; set; }

        [MaxLength(50)]
        public string RefCode { get; set; }

        public Guid RouteId { get; set; }

        [MaxLength(150)]
        public string RouteName { get; set; }

        public Guid TripId { get; set; }

        [MaxLength(300)]
        public string TripName { get; set; }

        public Guid? BusboyId { get; set; }

        [MaxLength(100)]
        public string BusboyUsername { get; set; }

        public Guid VehicleId { get; set; }

        [MaxLength(20)]
        public string VehicleRegistrationNumber { get; set; }

        public Guid DepartureTerminalId { get; set; }

        [MaxLength(200)]
        public string DepartureTerminalName { get; set; }

        public Guid DestinationPickupPointId { get; set; }

        [MaxLength(200)]
        public string DestinationPickupPointName { get; set; }
    }
}
