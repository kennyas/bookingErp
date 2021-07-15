using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Tornado.Shared.AuditLogEvent.Interface
{
    public interface IAuditLogEventService
    {
        Task PublishEvent(IntegrationEvent integration);
    }
}
