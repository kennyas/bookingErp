using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BusBoyOperationApiController : BaseController
    {
        private readonly IBookingService _bookingService;
        private readonly IBusBoyService _busBoyService;



        public BusBoyOperationApiController(
            IBookingService bookingService, IBusBoyService busBoyService)
        {
            _bookingService = bookingService;
            _busBoyService = busBoyService;
        }
        [HttpGet]

        public async Task<ApiResponse<string>> IsVehicleAttachedToBusBoy([FromQuery]VehicleAttachedViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () => {

                var validationResults = await _busBoyService.IsVehicleAttachedToBusboy(model)
                                                .ConfigureAwait(false);

                return new ApiResponse<string>(errors: validationResults.Select(p => p.ErrorMessage)
                    .ToArray());
            }).ConfigureAwait(false);


        }
        [HttpPost]

        public async Task<ApiResponse<string>> AttachVehicleToBusBoy(BusBoyVehicleAttachedViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () => {

                var validationResults = await _busBoyService.AttachVehicleToBusBoy(model)
                                                .ConfigureAwait(false);

                return new ApiResponse<string>(errors: validationResults.Select(p => p.ErrorMessage)
                    .ToArray());
            }).ConfigureAwait(false);


        }
        [HttpPost]
        public async Task<ApiResponse<BookingCreateResponseModel>> BookSeat(BusBoyBookingRequestModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<BookingCreateResponseModel>(errors: ListModelErrors.ToArray());

                var (validationResults, bookingResult) = await _bookingService.CreateBusBoyBooking(model).ConfigureAwait(false);

                if (!validationResults.Any())
                    return new ApiResponse<BookingCreateResponseModel>(bookingResult, "Booking successful");

                return new ApiResponse<BookingCreateResponseModel>(errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }
    }
}