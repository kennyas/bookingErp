using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Booking.Core.Services.Interfaces
{
    public interface IBookingEventService
    {
        Task PublishEvent(IntegrationEvent integration);
        Task PublishAndLogEvent(IntegrationEvent integration);
    }
}