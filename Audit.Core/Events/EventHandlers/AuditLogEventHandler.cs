using Audit.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Tornado.Shared.AuditLogEvent;
using Tornado.Shared.AzurePub.EventBus.Abstractions;

namespace Audit.Core.Events.EventHandlers
{
    public class AuditLogEventHandler : IIntegrationEventHandler<AuditLogIntegrationEvent>
    {
        private readonly IAuditService _auditService;
        private readonly IEventBus _eventBus;

        public AuditLogEventHandler(IAuditService auditService, IEventBus eventBus)
        {
            _auditService = auditService;
            _eventBus = eventBus;
        }

        public async Task Handle(AuditLogIntegrationEvent @event)
        {
            await _auditService.CreateAuditLogAsync(@event);
        }
    }
}
