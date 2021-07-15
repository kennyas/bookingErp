using Microsoft.AspNetCore.Mvc;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Report.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SalesDashboardApiController : BaseController
    {
        private readonly ISalesDashboardService _salesDashboardService;

        public SalesDashboardApiController(
     ISalesDashboardService salesDashboardService
     )
        {
            _salesDashboardService = salesDashboardService;
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<TotalSalesByRouteViewModel>>> GetTotalSalesByRoute([FromQuery] GetTotalSalesByRouteViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _salesDashboardService.GetTotalSalesByRoute(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<TotalSalesByTerminalViewModel>>> GetTotalSalesByTerminal([FromQuery] GetTotalSalesByTerminalViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _salesDashboardService.GetTotalSalesByTerminal(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<TotalSalesByTripViewModel>>> GetTotalSalesByTrip([FromQuery] GetTotalSalesByTripViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _salesDashboardService.GetTotalSalesByTrip(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<TotalSalesByVehicleViewModel>>> GetTotalSalesByVehicle([FromBody] GetTotalSalesByVehicleViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _salesDashboardService.GetTotalSalesByVehicle(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

    }
}
