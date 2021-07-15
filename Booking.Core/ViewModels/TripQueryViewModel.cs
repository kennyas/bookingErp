using System;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class TripQueryViewModel : BasePaginatedViewModel
    {
        [Required]
        public Guid DepartureId { get; set; }
        [Required]
        public Guid DestinationId { get; set; }
        public Guid RouteId { get; set; }

        //for bus boy bookings
        


        //public string DepartureDate { get; set; }
        //public string ReturnDate { get; set; }
        //public bool IsReturn { get; set; }
    }
}