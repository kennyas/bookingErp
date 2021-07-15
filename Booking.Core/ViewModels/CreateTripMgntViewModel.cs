using System;

namespace Booking.Core.ViewModels
{
    public class CreateTripManagementViewModel
    {
        public Guid TripId { get; set; }
        public DateTime DepartureDate { get; set; }
        public string DriverId { get; set; }
        public string BusBoyId { get; set; }
        public string VehicleId { get; set; }
        protected internal Guid Id { get; internal set; }
    }
}