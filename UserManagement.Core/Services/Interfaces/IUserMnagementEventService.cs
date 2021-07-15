using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace UserManagement.Core.Services.Interfaces
{
    public interface IUserMnagementEventService
    {
        Task PublishAndLogEvent(IntegrationEvent integration);

        Task PublishEvent(IntegrationEvent integration);


    }
}