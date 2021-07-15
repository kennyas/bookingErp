using Report.Core.Enums;
using Report.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Enums;

namespace Report.Core.Event.EventHandler
{
    public class BookingCompletedventHandler : IIntegrationEventHandler<BookingCompletedIntegrationEvent>
    {
        private readonly ICustomerBookingsReportService _customerBookingsReportService;

        public IBroadcastService _broadcastService { get; }

        public BookingCompletedventHandler(ICustomerBookingsReportService customerBookingsReportService, IBroadcastService broadcastService)
        {
            _customerBookingsReportService = customerBookingsReportService;
            _broadcastService = broadcastService;

        }

        public async Task Handle(BookingCompletedIntegrationEvent @event)
        {
            var bookingsDetails = (ViewModels.UpdateCustomerBookingsReportViewModel)@event;
            bookingsDetails.BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.APPROVED);
            bookingsDetails.PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID);

            // Set payment to default wallet because this event is only called when a customer successfully pays via his/her wallet
            bookingsDetails.PaymentMethod = Enum.GetName(typeof(PaymentMethod), PaymentMethod.WALLET);


            var serviceResponse = await _customerBookingsReportService.UpdateCustomerBookings(bookingsDetails);

            if (serviceResponse.Code == ApiResponseCodes.OK)
            {
                await _broadcastService.BroadcastToAllConnectedClients();
            }
        }


    }
}