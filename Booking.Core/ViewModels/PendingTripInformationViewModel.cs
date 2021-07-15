using Booking.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Booking.Core.ViewModels
{
    public class PendingTripInformationViewModel
    {
        internal IEnumerable<int> excludedSeats;
        internal IEnumerable<BookedSeat> bookedSeats;
        internal int seatsInVehicle;
        internal int departureOrderNo;

        public Guid Id { get; set; }
        public Guid TripId { get; set; }

        public int[] AvailableSeats
        {
            get
            {

                return Enumerable.Range(1, seatsInVehicle)
                                  .Except(excludedSeats)
                                  .Except(bookedSeats.Where(x => x.destinationOrderNumber > departureOrderNo).Select(x => x.seatNumber))
                                  .ToArray();
            }
        }
        public DateTime DepartureTime { get; internal set; }
        public TripManagementStatus Status { get; internal set; }
        public Guid DepartureRoutePointId { get; internal set; }
        public Guid DestinationRoutePointId { get; internal set; }
    }
}