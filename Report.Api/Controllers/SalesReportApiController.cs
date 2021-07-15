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
    public class SalesReportApiController : BaseController
    {
        private readonly IBusboySalesReportService _busboySalesReportService;
        private readonly ITripSalesReportService _tripSalesReportService;

        public SalesReportApiController(
     IBusboySalesReportService busboySalesReportService,
     ITripSalesReportService tripSalesReportService
     )
        {
            _busboySalesReportService = busboySalesReportService;
            _tripSalesReportService = tripSalesReportService;
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<TripSalesReportViewModel>>> GetTripSalesReport([FromQuery] GetTripSalesReportViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _tripSalesReportService.GetTripSalesReport(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }


        [HttpGet]
        public async Task<ApiResponse<PaginatedList<BusboySalesReportViewModel>>> GetBusboySalesReport([FromQuery] GetBusboySalesReportViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _busboySalesReportService.GetBusboySalesReport(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

    }
}
