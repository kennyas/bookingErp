using System;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Core.ViewModels
{
    public class TripSearchResponseViewModel
    {
        internal IEnumerable<int> excludedSeats;
        internal IEnumerable<BookedSeat> bookedSeats;
        internal int seatsInVehicle;
        internal int departureOrderNo;

        public int[] TotalAvailableSeats
        {
            get
            {
                return Enumerable.Range(1, seatsInVehicle)
                                 .Except(excludedSeats)
                                 .Except(bookedSeats.Where(x => x.destinationOrderNumber > departureOrderNo).Select(x => x.seatNumber))
                                 .ToArray();
            }
        }
        public string Departure { get; internal set; }
        public string Destination { get; internal set; }
        public string DepartureDate { get; internal set; }
        public string DepartureTime { get; internal set; }
        public Guid Id { get; internal set; }
        public decimal Amount { get; internal set; }
        public string RouteName { get; internal set; }
        public Guid DestinationId { get; internal set; }
        public Guid DepartureId { get; internal set; }
        public string VehicleModelTitle { get;  set; }
        //public Guid VehicleModelId { get; set; }


    }

    public class BookedSeat
    {
        internal int seatNumber;
        internal int destinationOrderNumber;
    }
}