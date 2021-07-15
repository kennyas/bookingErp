using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Policy;
using Tornado.Shared.EF;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RoutePickupPointController : BaseController
    {
        private readonly IRoutePickupPointService _routePickupPointService;
        public RoutePickupPointController(IRoutePickupPointService routePickupPointService)
        {
            _routePickupPointService = routePickupPointService;
        }

        [HttpPost]
        public async Task<ApiResponse<RoutePickupPointViewModel>> AddRoutePickupPoint(CreateRoutePickupPointViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routePickupPointService.AddPickupPointToRoute(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<RoutePickupPointViewModel>>> GetRoutePickupPoints([FromQuery]GetRoutePickupPoints model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routePickupPointService.GetRoutePickupPoints(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
       
        [HttpDelete]
        public async Task<ApiResponse<RoutePickupPointViewModel>> DeleteRoutePickupPoint([FromQuery] DeleteRoutePickupPointsById model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routePickupPointService.RemovePickupPointToRoute(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }   
        
        [HttpDelete]
        public async Task<ApiResponse<RoutePickupPointViewModel>> DeleteAllRoutePickupPoint([FromBody] DeleteAllRoutePickupPointsById model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routePickupPointService.RemoveAllRoutePickupPoints(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<RoutePickupPointViewModel>>> GetDestinationRoutePickupPointbyDeparture([FromQuery]GetRoutePickupPoints model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routePickupPointService.GetRoutePickupPoints(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
