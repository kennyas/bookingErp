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
    public class BookingsReportApiController : BaseController
    {
        private readonly ICustomerBookingsReportService _customerBookingReportService;
        private readonly IBookedBusesReportService _bookedBusesReportService;
        private readonly IBookedTicketsReportService _bookedTicketsReportService;

        public BookingsReportApiController(
            ICustomerBookingsReportService bookingReportService,
            IBookedBusesReportService bookedBusesReportService,
            IBookedTicketsReportService bookedTicketsReportService
            )
        {
            _customerBookingReportService = bookingReportService;
            _bookedBusesReportService = bookedBusesReportService;
            _bookedTicketsReportService = bookedTicketsReportService;
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<CustomerBookingsReportViewModel>>> GetCustomerBookings([FromQuery] GetCustomerBookingsReportViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _customerBookingReportService.GetCustomerBookings(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<BookedTicketReportViewModel>>> GetBookedTickets([FromQuery] GetBookedTicketReportViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _bookedTicketsReportService.GetBookedTickets(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<BookedBusReportViewModel>>> GetBookedBuses([FromQuery] GetBookedBusReportViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _bookedBusesReportService.GetBookedBuses(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}