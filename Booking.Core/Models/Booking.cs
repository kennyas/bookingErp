using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Booking : BaseEntity
    {
        public bool IsMainBooking { get; set; }
        [MaxLength(300)]
        public string MainBookingRefCode { get; set; }
        public Guid TripManagementId { get; set; }
        [ForeignKey(nameof(TripManagementId))]
        public TripManagement TripManagement { get; set; }
        public Guid? CustomerId { get; set; }
        [MaxLength(320)]
        public string BookingEmail { get; set; }
        [MaxLength(50)]
        public string RefCode { get; set; }
        public CheckInStatus CheckInStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public BookingType BookingType { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public int SeatNumber { get; set; }
        [MaxLength(50)]
        public string BookingPhoneNumber { get; set; }
        [MaxLength(100)]
        public string BookingFirstName { get; set; }
        [MaxLength(100)]
        public string BookingLastName { get; set; }
        public Guid DepartureId { get; set; }
        public RoutePoint Departure { get; set; }
        public Guid DestinationId { get; set; }
        public RoutePoint Destination { get; set; }
        public decimal Amount { get; set; }


        //from GIGL or GIGL
        public string GIGRefCode { get; set; }
    }
}