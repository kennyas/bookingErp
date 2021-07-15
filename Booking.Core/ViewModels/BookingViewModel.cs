using Booking.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class BookingRequestModel : IValidatableObject
    {
        [Required]
        public int[] Seats { get; set; }
        [Required]
        public Guid TripId { get; set; }
        [Required]
        public Guid DepartureId { get; set; }
        [Required]
        public Guid DestinationId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Seats.Length <= 0)
            {
                yield return new ValidationResult("Please select at least a seat");
            }
            if (Seats.Distinct().Count() != Seats.Length)
            {
                yield return new ValidationResult("Duplicate seat cannot be selected.");
            }

            if (DepartureId.Equals(DestinationId))
            {
                yield return new ValidationResult("Departure and Destination must be different.");
            }
        }
    }

    public enum BusBoyPaymentMethod
    {
        CASH, POS, ON_ARRIVAL
    }

    public class BusBoyBookingRequestModel : IValidatableObject
    {
        [Required]
        public int[] Seats { get; set; }
        [Required]
        public Guid TripId { get; set; }
        [Required]
        public Guid DepartureId { get; set; }
        [Required]
        public Guid DestinationId { get; set; }

        public BusBoyPaymentMethod PaymentMethod { get; set; }

        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Seats.Length <= 0)
            {
                yield return new ValidationResult("Please select at least a seat");
            }
            if (Seats.Distinct().Count() != Seats.Length)
            {
                yield return new ValidationResult("Duplicate seat cannot be selected.");
            }

            if (DepartureId.Equals(DestinationId))
            {
                yield return new ValidationResult("Departure and Destination must be different.");
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                yield return new ValidationResult($"{nameof(LastName)} required.");
            }

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                yield return new ValidationResult($"{nameof(FirstName)} required.");
            }

            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                yield return new ValidationResult($"{nameof(PhoneNumber)} required.");
            }
        }
    }

    public class BookingDetailsModel
    {
        public string RefCode { get; internal set; }
        public string Status { get; internal set; }
        public int SeatNumber { get; internal set; }
        public decimal Amount { get; internal set; }
        public string DateBooked { get; internal set; }
        public string Departure { get; internal set; }
        public string Destination { get; internal set; }
        public string PaymentStatus { get; internal set; }
        public string DepartureDate { get; internal set; }
        public string DepartureTime { get; internal set; }
    }

    public class EntityBookingDetailsModel
    {
        internal IEnumerable<string> additionalSeats;

        public string RefCode { get; set; }
        public BookingStatus Status { get; set; }
        public string MainSeatNumber { get; set; }
        public string SubSeatNumbers => string.Join(", ", additionalSeats.Select(i => i.ToString()));
        public decimal Amount { get; set; }
        public DateTime DateBooked { get; set; }
        public string Departure { get; set; }
        public string Destination { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public Guid Id { get; set; }
    }

    public class BookingCreateResponseModel
    {
        public List<SeatRefcodeModel> SeatRefCode { get; internal set; } = new List<SeatRefcodeModel>();
        public decimal AmountPayble { get; internal set; }
        public string PaymentStatus { get; internal set; }
        public string BookingStatus { get; internal set; }
        public string BookingDate { get; internal set; }

        public List<int> SelectedSeats { get; set; } = new List<int>();
    }

    public class SeatRefcodeModel
    {
        public string Refcode { get; set; }
        public int Seat { get; set; }
    }

    public class CustomerBookingSearchModel : BasePageQueryViewModel
    {
        public Guid CustomerId { get; set; }
    }

    public class PhoneNumberBookingSearchModel : BasePageQueryViewModel
    {
        public string PhoneNumber { get; set; }
    }

    public class CancelBookingModel
    {
        public string Refcode { get; set; }
        public string Reason { get; set; }
    }

    public class BillBookingModel
    {
        public string Refcode { get; set; }
        public Guid CustomerId { get; set; }
    }
}