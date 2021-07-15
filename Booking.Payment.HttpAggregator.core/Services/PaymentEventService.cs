using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Booking.Payment.HttpAggregator.core.Services
{
    public interface IPaymentEventService
    {
        Task PublishEvent(IntegrationEvent integration);
    }

    public class PaymentEventService : IPaymentEventService
    {
        private readonly IEventBus _eventBus;

        public PaymentEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task PublishEvent(IntegrationEvent integration)
        {
            return Task.Run(() => _eventBus.Publish(integration));
        }
    }
}