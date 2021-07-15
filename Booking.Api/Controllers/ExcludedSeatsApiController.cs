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
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]

    public class ExcludedSeatsApiController : BaseController
    {
        private readonly IVehicleExcludedSeatService _vehicleExcludedSeatService;
        public ExcludedSeatsApiController(IVehicleExcludedSeatService vehicleExcludedSeatService)
        {
            _vehicleExcludedSeatService = vehicleExcludedSeatService;
        }

        [HttpPost]
        public async Task<ApiResponse<string>> UpdateExcludedSeats(AddVehicleExcludedSeatsRequestViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<string>(errors: "Request is invalid, Please confirm and try again.");

                var results = await _vehicleExcludedSeatService.UpdateVehicleExcludedSeats(model).ConfigureAwait(false);

                if (!results.Any())
                    return new ApiResponse<string>("Seats updated successfully.");

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<VehicleExcludedSeatsViewModel>> GetSeatDetails(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var results = await _vehicleExcludedSeatService.GetVehicleExcludedSeat(id).ConfigureAwait(false);
                return results;
            }).ConfigureAwait(false);
        }

    }
}
