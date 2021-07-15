using Booking.Core.Helpers;
using Booking.Core.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;

namespace Booking.Core.Events.EventHandlers
{
    public class PaymentSucceededEventHandler : IIntegrationEventHandler<PaymentSucceededIntegrationEvent>
    {
        private readonly IBookingService _bookingService;
        private readonly IEventBus _eventBus;

        public PaymentSucceededEventHandler(IBookingService bookingService, IEventBus eventBus)
        {
            _bookingService = bookingService;
            _eventBus = eventBus;
        }

        public async Task Handle(PaymentSucceededIntegrationEvent @event)
        {
            if (@event.TransType == CoreConstants.TransactionType.WalletDebit)
            {
                var result = await _bookingService.UpdateBookingOnSuccessfulPaymentEvent(@event);

                if (!result.Any())
                {
                    var booking = await _bookingService.GetBookingByRefCode(@event.PaymentRefCode);

                    var childSeats = await _bookingService.GetChildSeats(@event.PaymentRefCode);

                    var bookedSeats = string.Join(",", $"{booking.SeatNumber}", childSeats);

                    _eventBus.Publish(new BookingCompletedIntegrationEvent(@event.PaymentRefCode,
                        booking.DepartureDate, booking.DepartureTime, bookedSeats, @event.CustomerEmail,
                        booking.Departure, booking.Destination, @event.Amount,@event.CustomerPhoneNumber));
                }
            }
        }
    }
}