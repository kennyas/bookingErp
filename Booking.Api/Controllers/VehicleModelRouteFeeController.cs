using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]

    public class VehicleModelRouteFeeController : BaseController
    {

        public readonly IVehicleModelRouteFeeService _vehicleModelRouteFeeService;
        public VehicleModelRouteFeeController(IVehicleModelRouteFeeService vehicleModelRouteFeeService)
        {
           _vehicleModelRouteFeeService = vehicleModelRouteFeeService;
        }
        [HttpGet]
        public async Task<ApiResponse<List<VehicleModelRouteFeeViewModel>>> GetAllVehicleModelRouteFees([FromQuery] VehicleModelRouteFeeRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<List<VehicleModelRouteFeeViewModel>>(errors: ListModelErrors.ToArray());
            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _vehicleModelRouteFeeService.GetAllVehicleModelRouteFees(model, out int totalCount)
                                                      .ConfigureAwait(false);

                return new ApiResponse<List<VehicleModelRouteFeeViewModel>>
                (bookings, totalCount: totalCount);

            }).ConfigureAwait(false);
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<string>> CreateSubTripVehicleModelRouteFee(CreateVehicleModelRouteFeeViewModel model)
        {

            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var validationResults = await _vehicleModelRouteFeeService.CreateVehicleModelRouteFee(model).ConfigureAwait(false);

                return new ApiResponse<string>
                (errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }
        //[HttpDelete]
        [HttpGet]
        public async Task<ApiResponse<string>> DeleteVehicleModelRouteFee(string id)
        {

            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var validationResults = await _vehicleModelRouteFeeService.DeleteVehicleModelRouteFee(id)
                                .ConfigureAwait(false);

                return new ApiResponse<string>
                (errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }

        //[HttpDelete]
        [HttpPost]
        public async Task<ApiResponse<string>> EditVehicleModelRouteFee(EditVehicleModelRouteFeeViewModel model)
        {

            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var validationResults = await _vehicleModelRouteFeeService.EditVehicleModelRouteFee(model)
                                .ConfigureAwait(false);

                return new ApiResponse<string>
                (errors: validationResults.Select(x => x.ErrorMessage).ToArray());

            }).ConfigureAwait(false);
        }
        //[HttpDelete]
        [HttpGet]
        public async Task<ApiResponse<SubRouteFeeViewModel>> GetVehicleModelRouteFee(string id)
        {


            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _vehicleModelRouteFeeService.GetVehicleModelRouteFee(id)
                                .ConfigureAwait(false);

                return new ApiResponse<SubRouteFeeViewModel>();

            }).ConfigureAwait(false);
        }
    }
}

