using Microsoft.AspNetCore.Mvc;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Report.Api.Controllers
{
    //For polling use only
    [Route("api/[controller]/[action]")]
    public class BookingsDashboardApiController : BaseController
    {
        private readonly ICustomerBookingsDashboardService _customerBookingsDashboardService;

        public BookingsDashboardApiController(ICustomerBookingsDashboardService customerBookingsDashboardService)
        {
            _customerBookingsDashboardService = customerBookingsDashboardService;
        }

        [HttpGet]
        public async Task<ApiResponse<CustomerBookingsDashboardWithDataViewModel>> GetCustomerBookings([FromQuery] GetCustomerBookingsDashboardByDateViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _customerBookingsDashboardService.GetCustomerBookingsDashboardByDate(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}