using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Policy;
using Tornado.Shared.EF;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RouteApiController : BaseController
    {
        private readonly IRouteService _routeService;
        public RouteApiController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpPost]
        [RequiresPermission(Permission.CREATE_ROUTE)]
        public async Task<ApiResponse<CreateRouteViewModel>> AddRoute(CreateRouteViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeService.AddRoute(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<RouteViewModel>> GetRoute(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeService.GetRoute(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        //[HttpPut]
        [HttpPut]
        [RequiresPermission(Permission.EDIT_ROUTE)]
        public async Task<ApiResponse<RouteViewModel>> EditRoute(EditRouteViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeService.EditRoute(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<PaginatedList<RouteListViewModel>>> GetAllRoutes (BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _routeService.GetAllRoutesAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}