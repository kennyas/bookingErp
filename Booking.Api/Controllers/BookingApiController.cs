using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Enums;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BookingApiController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IHttpUserService _currentUserService;

        public BookingApiController(
            IBookingService bookingService,
            IHttpUserService currentUserService)
        {
            _bookingService = bookingService;
            _currentUserService = currentUserService;
        }

        [HttpPost]
        public async Task<ApiResponse<CancelBookingModel>> CancelBooking(CancelBookingModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<CancelBookingModel>(errors: ListModelErrors.ToArray());

                var validationResults = await _bookingService.CancelBooking(model).ConfigureAwait(false);

                if (!validationResults.Any())
                    return new ApiResponse<CancelBookingModel>();

                return new ApiResponse<CancelBookingModel>(errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<BookingCreateResponseModel>> BookSeat(BookingRequestModel model)
        {
            return await HandleApiOperationAsync(async () =>
                      {
                          if (!ModelState.IsValid)
                              return new ApiResponse<BookingCreateResponseModel>(errors: ListModelErrors.ToArray());

                          var (validationResults, bookingResult) = await _bookingService.CreateBooking(model).ConfigureAwait(false);

                          if (!validationResults.Any())
                              return new ApiResponse<BookingCreateResponseModel>(bookingResult, "Booking successful");

                          return new ApiResponse<BookingCreateResponseModel>(errors: validationResults.Select(x => x.ErrorMessage).ToArray());

                      }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<List<BookingDetailsModel>>> GetUserBookings([FromQuery] BasePageQueryViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<List<BookingDetailsModel>>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var search = new CustomerBookingSearchModel {
                    PageIndex = model.PageIndex,
                    PageTotal = model.PageTotal,
                    CustomerId = _currentUserService.GetCurrentUser().UserId
                };

                var bookings = await _bookingService.GetCustomerBookingsByCustomer(search, out int totalCount)
                                                    .ConfigureAwait(false);

                return new ApiResponse<List<BookingDetailsModel>>(bookings, totalCount: totalCount);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<List<BookingDetailsModel>>> GetUserBookingsByPhoneNumber([FromQuery] PhoneNumberBookingSearchModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<List<BookingDetailsModel>>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {

                var bookings = await _bookingService.GetCustomerBookingsByPhoneNumber(model, out int totalCount)
                                                    .ConfigureAwait(false);

                return new ApiResponse<List<BookingDetailsModel>>(bookings, totalCount: totalCount);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<decimal?>> GetPointFare([FromQuery] Guid tripId, Guid departureId, Guid destinationId)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<decimal?>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {

                var bookings = _bookingService.ComputeteFare(tripId,departureId,destinationId);

                return new ApiResponse<decimal?>(bookings);

            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<string>> CheckInRefcode(BookingCheckStatusModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var validationResults = await _bookingService.CheckinRefcode(model.RefCode)
                                                    .ConfigureAwait(false);

                if (!validationResults.Any())
                    return new ApiResponse<string>("Check-In successful");

                return new ApiResponse<string>(errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<string>> CheckOutRefcode(BookingCheckStatusModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var validationResults = await _bookingService.CheckoutRefCode(model.RefCode)
                                                    .ConfigureAwait(false);

                if (!validationResults.Any())
                    return new ApiResponse<string>("Check-Out successful");

                return new ApiResponse<string>(errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<BookingDetailsModel>> GetBooking([Required] string refCode)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<BookingDetailsModel>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var booking = await _bookingService.GetBookingByRefCode(refCode)
                                                 .ConfigureAwait(false);
                if (booking is null)
                    return new ApiResponse<BookingDetailsModel>(errors: "record not found", codes: ApiResponseCodes.NOT_FOUND);

                return new ApiResponse<BookingDetailsModel>(booking);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<EntityBookingDetailsModel>> GetEntityBooking([Required] string refCode)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<EntityBookingDetailsModel>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var booking = await _bookingService.GetEntityBookingByRefCode(refCode)
                                                 .ConfigureAwait(false);
                if (booking is null)
                    return new ApiResponse<EntityBookingDetailsModel>(errors: "record not found", codes: ApiResponseCodes.NOT_FOUND);

                return new ApiResponse<EntityBookingDetailsModel>(booking);

            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<bool>> VerifyAndPayViaRefCode
            (UpdateBookingByExternalRefViewModel model)
        {

            return await HandleApiOperationAsync(async () =>
            {

                if (!ModelState.IsValid)
                    return new ApiResponse<bool>(errors: ListModelErrors.ToArray());

                var validationResults = await _bookingService.BookSeatsViaExternalRefAsync(model).ConfigureAwait(false);
                var isVerified = !(validationResults.Any());

                return new ApiResponse<bool>
                    (data: isVerified, errors: validationResults.Select(x => x.ErrorMessage).ToArray(),
                        message: isVerified ? "Ref code is valid" : "Ref code is invalid" );

            }).ConfigureAwait(false);
        }
    }
}