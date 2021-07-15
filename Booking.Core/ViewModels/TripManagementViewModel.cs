using System;

namespace Booking.Core.ViewModels
{
    public class TripManagementViewModel
    {
        public string BusBoy { get; internal set; }
        public Guid Id { get; internal set; }
        public string Driver { get; internal set; }
        public string DepartureDate { get; internal set; }
        public string Status { get; internal set; }
        public decimal? BaseFee { get; internal set; }
        public Guid TripId { get; internal set; }
    }
}