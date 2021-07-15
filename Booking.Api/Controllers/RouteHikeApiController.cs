using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class RouteHikeApiController : BaseController
    {
        private readonly IRouteHikeService _routeHikeService;

        public RouteHikeApiController(IRouteHikeService routeHikeService)
        {
            _routeHikeService = routeHikeService;
        }

        [HttpPost]
        public async Task<ApiResponse<RouteHikeCreateViewModel>> CreateRouteHike([FromBody]RouteHikeCreateViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeHikeService.CreateRouteHikeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }


        [HttpPost]
        public async Task<ApiResponse<RouteHikeEditViewModel>> EditRouteHike([FromBody]RouteHikeEditViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeHikeService.EditRouteHikeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ApiResponse<RouteHikeEditViewModel>> DeleteRouteHike(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeHikeService.DeleteRouteHikeAsync(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<RouteHikeCreateViewModel>> GetRouteHike(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeHikeService.GetRouteHikeAsync(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        //[HttpPost]
        //public async Task<ApiResponse<PaginatedList<RouteHike>>> GetAllRouteHike([FromBody]BaseSearchViewModel model)
        //{
        //    return await HandleApiOperationAsync(async () =>
        //    {
        //        return await _RouteHideService.GetAllRouteHideAsync(model).ConfigureAwait(false);
        //    }).ConfigureAwait(false);
        //}
    }
}