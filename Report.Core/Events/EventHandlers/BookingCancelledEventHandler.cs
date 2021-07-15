using Report.Core.Enums;
using Report.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Enums;

namespace Report.Core.Event.EventHandler
{
    public class BookingCancelledEventHandler : IIntegrationEventHandler<BookingCancelledIntegrationEvent>
    {
        private readonly ICustomerBookingsReportService _customerBookingsReportService;
        private readonly IBroadcastService _broadcastService;

        public BookingCancelledEventHandler(ICustomerBookingsReportService customerBookingsReportService, IBroadcastService broadcastService)
        {
            _customerBookingsReportService = customerBookingsReportService;
            _broadcastService = broadcastService;
        }

        public async Task Handle(BookingCancelledIntegrationEvent @event)
        {
            var bookingsDetails = (ViewModels.UpdateCustomerBookingsReportViewModel)@event;

            bookingsDetails.BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.CANCELLED);

            var serviceResponse = await _customerBookingsReportService.UpdateCustomerBookings(bookingsDetails);

            if (serviceResponse.Code == ApiResponseCodes.OK)
            {
               await _broadcastService.BroadcastToAllConnectedClients();
            }
        }
    }
}
