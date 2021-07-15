using Booking.Core.Events;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tornado.Shared.EF.Services;

namespace Booking.Core.Services.Interfaces
{
    public interface IBookingService : IService<Models.Booking>
    {
        Task<List<ValidationResult>> UpdateBookingOnSuccessfulPaymentEvent(PaymentSucceededIntegrationEvent model);
        Task<List<BookingDetailsModel>> GetCustomerBookingsByCustomer(
            CustomerBookingSearchModel search,
            out int totalCount);
        Task<BookingDetailsModel> GetBookingByRefCode(string refCode);
        Task<EntityBookingDetailsModel> GetEntityBookingByRefCode(string refCode);
        Task<(List<ValidationResult> validationResults, BookingCreateResponseModel bookingResult)> 
            CreateBooking(BookingRequestModel model);
        Task<(List<ValidationResult> validationResults, BookingCreateResponseModel bookingResult)>
            CreateBusBoyBooking(BusBoyBookingRequestModel model);
        Task<List<ValidationResult>> CancelBooking(CancelBookingModel model);
        Task<List<ValidationResult>> CheckinRefcode(string refcode);
        Task<List<ValidationResult>> CheckoutRefCode(string refcode);
        Task<List<string>> GetChildSeats(string refCode);
        Task<List<BookingDetailsModel>> GetCustomerBookingsByPhoneNumber(
            PhoneNumberBookingSearchModel search,
            out int totalCount);


        //Task<List<ValidationResult>> VerifyGIGLRefcode(string refcode);
        Task<List<ValidationResult>> BookSeatsViaExternalRefAsync(UpdateBookingByExternalRefViewModel mdoel);
        decimal? ComputeteFare(Guid tripId, Guid departureId, Guid destinationId);
    }
}