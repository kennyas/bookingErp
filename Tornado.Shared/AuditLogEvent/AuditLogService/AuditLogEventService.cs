using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AuditLogEvent.Interface;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Tornado.Shared.AuditLogEvent.AuditLogService
{

    public class AuditLogEventService : IAuditLogEventService
    {
        private readonly IEventBus _eventBus;

        public AuditLogEventService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        public Task PublishEvent(IntegrationEvent integration)
        {
            return Task.Run(() => _eventBus.Publish(integration));
        }
    }
}

