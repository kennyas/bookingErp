using Report.Core.Enums;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Enums;

namespace Report.Core.Event.EventHandler
{
    public class BookingCreatedEventHandler : IIntegrationEventHandler<BookingCreatedIntegrationEvent>
    {
        private readonly ICustomerBookingsReportService _customerBookingsReportService;
        private readonly IBroadcastService _broadcastService;

        public BookingCreatedEventHandler(ICustomerBookingsReportService customerBookingsReportService, IBroadcastService broadcastService)
        {
            _customerBookingsReportService = customerBookingsReportService;
            _broadcastService = broadcastService;
        }

        public async Task Handle(BookingCreatedIntegrationEvent @event)
        {
            var newBooking = (AddCustomerBookingsReportViewModel)@event;

            if (string.IsNullOrEmpty(newBooking.BookingStatus))
                newBooking.BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.PENDING);

            var serviceResponse = await _customerBookingsReportService.AddCustomerBookings(newBooking);

            if (serviceResponse.Code == ApiResponseCodes.OK)
            {
                await _broadcastService.BroadcastToAllConnectedClients();
            }
        }
    }
}
