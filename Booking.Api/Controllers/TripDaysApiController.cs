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
    public class TripDaysApiController : BaseController
    {
        private readonly ITripDaysService _tripDaysService;

        public TripDaysApiController(ITripDaysService tripDaysService)
        {
            _tripDaysService = tripDaysService;
        }
        [HttpPost]
        public async Task<ApiResponse<TripDaysViewModel>> AddTripDays(TripDaysRequestModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripDaysService.CreateTripDays(model).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<List<TripDaysDetailViewModel>>> AllTripDays([FromQuery]TripDaysSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripDaysService.AllTripDays(model).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }
        //[HttpDelete]
        //public async Task<ApiResponse<TripDaysViewModel>> DeleteTripDays(string guidId)
        //{
        //return await HandleApiOperationAsync(async () =>
        //{
        //    //return await _tripDaysService.CreateTripDays(model).ConfigureAwait(false);

        //}).ConfigureAwait(false);
        // }
    }
}