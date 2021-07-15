using System;

namespace Booking.Core.ViewModels
{
    public class TripRouteViewModel
    {
        public Guid Id { get; set; }
        public Guid TripId { get; set; }
        public Guid RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDescription { get; set; }
        public bool IsMainRoute { get; internal set; }
        public decimal Amount { get; internal set; }
    }
}