using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CaptainOperationApiController : BaseController
    {
        public readonly ICaptainService _captainService;
        public CaptainOperationApiController(ICaptainService captainService)
        {
            _captainService = captainService;
        }
        [HttpGet]
        public async Task<ApiResponse<string>> IsVehicleAttachedToCaptain([FromQuery]VehicleAttachedViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () => {

                var validationResults = await _captainService.IsVehicleAttachedToCaptain(model)
                                                .ConfigureAwait(false);

                return new ApiResponse<string>(errors: validationResults.Select(p => p.ErrorMessage)
                    .ToArray());
            }).ConfigureAwait(false);


        }
        //[HttpPut]
        [HttpPost]

        public async Task<ApiResponse<string>> AttachVehicleToCaptain([FromQuery]CaptainVehicleAttachedViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () => {

                var validationResults = await _captainService.AttachedToCaptain(model)
                                                .ConfigureAwait(false);

                return new ApiResponse<string>(errors: validationResults.Select(p => p.ErrorMessage)
                    .ToArray());
            }).ConfigureAwait(false);


        }

    }
}
