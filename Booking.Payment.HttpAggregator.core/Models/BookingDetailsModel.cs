using System;

namespace Booking.Payment.HttpAggregator.core.Models
{
    public class BookingEntityModel
    {
        public Guid Id { get; set; }
        public string RefCode { get;  set; }
        public BookingStatus Status { get;  set; }
        public string MainSeatNumber { get; set; }
        public string SubSeatNumbers { get; set; }
        public decimal Amount { get;  set; }
        public DateTime DateBooked { get;  set; }
        public string Departure { get;  set; }
        public string Destination { get;  set; }
        public PaymentStatus PaymentStatus { get;  set; }
        public Guid CustomerId { get;  set; }
        public string CustomerEmail  { get; set; }

        public string GIGRefCode { get; set; }
        public string Channel { get; set; }

    }


    public class BookingDetailsModel
    {
        public string RefCode { get;  set; }
        public string Status { get;  set; }
        public int SeatNumber { get;  set; }
        public decimal Amount { get;  set; }
        public string DateBooked { get;  set; }
        public string Departure { get;  set; }
        public string Destination { get;  set; }
        public string PaymentStatus { get;  set; }
    }
}