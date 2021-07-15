using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class BusBoySearchTripsResponseViewModel
    {
        public Guid Id { get; internal set; }

        internal IEnumerable<int> excludedSeats;
        internal IEnumerable<BookedSeat> bookedSeats;
        internal int seatsInVehicle;

        public string RouteName { get; internal set; }
        public string DepartureTime { get; internal set; }
        public string DepartureDate { get; internal set; }
        public string VehicleModelTitle { get; set; }
        public decimal BaseAmount { get; internal set; }

    }
}
