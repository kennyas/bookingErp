using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SubRouteFeeController : BaseController
    {
        public readonly ISubRouteFeeService _subRouteFeeService;
        public SubRouteFeeController(ISubRouteFeeService subTripFeeService)
        {
            _subRouteFeeService = subTripFeeService;
        }
        [HttpGet]
        public async Task<ApiResponse<List<SubRouteFeeViewModel>>> GetAllSubTripFees([FromQuery] SubRouteFeeRequestViewModel model)
        {
            if (!ModelState.IsValid)
                return new ApiResponse<List<SubRouteFeeViewModel>>(errors: ListModelErrors.ToArray());
            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _subRouteFeeService.GetAllSubRouteFees(model, out int totalCount)
                                                      .ConfigureAwait(false);

                return new ApiResponse<List<SubRouteFeeViewModel>>(bookings, totalCount: totalCount);

            }).ConfigureAwait(false);
        }
        [HttpPost]
        public async Task<ApiResponse<string>> CreateSubTrip(CreateSubRouteFeeRequestViewModel model)
        {

            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _subRouteFeeService.CreateSubRouteFee(model).ConfigureAwait(false);

                return new ApiResponse<string>();

            }).ConfigureAwait(false);
        }
        //[HttpDelete]
        [HttpGet]
        public async Task<ApiResponse<string>> DeleteSubRouteFee(string id)
        {

            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _subRouteFeeService.DeleteSubRouteFee(id)
                                .ConfigureAwait(false);

                return new ApiResponse<string>();

            }).ConfigureAwait(false);
        }

        //[HttpDelete]
        [HttpPost]
        public async Task<ApiResponse<string>> EditSubRouteFee(EditSubRouteFeeRequestViewModel model)
        {

            if (!ModelState.IsValid)
                return new ApiResponse<string>(errors: ListModelErrors.ToArray());

            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _subRouteFeeService.EditSubRouteFee(model)
                                .ConfigureAwait(false);

                return new ApiResponse<string>();

            }).ConfigureAwait(false);
        }
        //[HttpDelete]
        [HttpGet]
        public async Task<ApiResponse<SubRouteFeeViewModel>> GetSubRouteFee(string id)
        {


            return await HandleApiOperationAsync(async () =>
            {
                var bookings = await _subRouteFeeService.GetSubRouteFee(id)
                                .ConfigureAwait(false);

                return new ApiResponse<SubRouteFeeViewModel>();

            }).ConfigureAwait(false);
        }
    }
}
