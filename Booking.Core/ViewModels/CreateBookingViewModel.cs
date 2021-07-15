using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class CreateBookingViewModel : BaseViewModel
    {
        public CreateBookingViewModel()
        {
            SeatNumbers = new List<int> { };
        }
        public string UserId { get; set; }
        public int BookingStatus { get; set; }
        public int CheckInStatus { get; set; }
        public List<int> SeatNumbers { get; set; }
        public string TripManagementId { get; set; }

        public int PaymentMethod { get; set; }
        public int BookingType { get; set; }


        //public explicit operator CreateBookingViewModel(BookingViewModel source)
        //{

        //}

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrEmpty(this.UserId) || !Guid.TryParse(this.UserId, out Guid userId))
            {
                yield return new ValidationResult("A valid user id is required.");
            }
            if (string.IsNullOrEmpty(this.TripManagementId) || !Guid.TryParse(this.TripManagementId, out Guid tripManagementId))
            {
                yield return new ValidationResult("No trip is attached to this.");
            }
        }
    }
}
