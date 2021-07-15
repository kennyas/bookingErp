using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TripApiController : BaseController
    {
        private readonly ITripService _tripService;

        public TripApiController(ITripService tripService)
        {
            _tripService = tripService;
        }
        [HttpGet]
        public async Task<ApiResponse<List<BusBoySearchTripsResponseViewModel>>> GetTodaysBusBoyTrips()
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _tripService.GetAllTodaysBusBoyTrips( out int totalCount).ConfigureAwait(false);

                return new ApiResponse<List<BusBoySearchTripsResponseViewModel>>(result, totalCount: totalCount);

            }).ConfigureAwait(false);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<List<TripSearchResponseViewModel>>> SearchTrips([FromQuery] TripQueryViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _tripService.SearchTrips(model, out int totalCount).ConfigureAwait(false);

                return new ApiResponse<List<TripSearchResponseViewModel>>(result, totalCount: totalCount);

            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<List<TripSearchResponseViewModel>>> SearchTripsForBusBoy([FromQuery] TripQueryViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _tripService.SearchTripForBusBoy(model, out int totalCount).ConfigureAwait(false);

                return new ApiResponse<List<TripSearchResponseViewModel>>(result, totalCount: totalCount);

            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetAllTrip([FromQuery]GetAllTripViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.GetAllTrip(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }  
        
        [HttpGet]
        public async Task<ApiResponse<ViewTripViewModel>> GetTripById([FromQuery]GetTripByIdViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.GetTripById(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

       

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsWithActiveDiscount([FromQuery]GetTripsWithActiveDiscountViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.GetTripsWithActiveDiscount(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsByVehicleId([FromQuery]GetTripsByVehicleIdViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.GetTripsByVehicleId(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsByVehicleRegistrationNumber([FromQuery]GetTripsByVehicleRegNoViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.GetTripsByRegistrationNumber(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        [HttpPost]
        public async Task<ApiResponse<TripViewModel>> CreateTrip([FromBody]TripViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.CreateTrip(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }


        [HttpPut]
        public async Task<ApiResponse<ViewTripViewModel>> EditTrip([FromBody]EditTripViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.EditTrip(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }


        [HttpPut]
        public async Task<ApiResponse<ViewTripViewModel>> EditTripDiscount([FromBody]TripDiscountViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.EditTripDiscount(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ApiResponse<ViewTripViewModel>> DeleteTrip([FromQuery]DeleteTripViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.DeleteTrip(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ApiResponse<ViewTripViewModel>> DeleteTripChildrenFee([FromQuery]DeleteTripChildrenFeeViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.DeleteTripChildrenFee(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ApiResponse<ViewTripViewModel>> DeleteTripDiscount([FromBody]DeleteTripDiscountViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripService.DeleteTripDiscount(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}