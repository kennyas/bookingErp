using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;
using Tornado.Shared.IntegrationEventLogEF.Services;
using Tornado.Shared.IntegrationEventLogEF.Utilities;
using UserManagement.Core.Context;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Core.Services
{
    public class UserMangementEventService : IUserMnagementEventService
    {

        private readonly IIntegrationEventLogService _eventLogService;
        private readonly IEventBus _eventBus;
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;
        private readonly GigAuthDbContext _gigAuthContext;
        public UserMangementEventService(IEventBus eventBus,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory,
            GigAuthDbContext gigAuthContext)
        {
            _eventBus = eventBus;
            _gigAuthContext = gigAuthContext ?? throw new ArgumentNullException(nameof(gigAuthContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory ?? throw new ArgumentNullException(nameof(integrationEventLogServiceFactory));
            _eventLogService = _integrationEventLogServiceFactory(_gigAuthContext.Database.GetDbConnection());
        }

        public async Task PublishAndLogEvent(IntegrationEvent integration)
        {
            try
            {
                await ResilientTransaction.New(_gigAuthContext).ExecuteAsync(async () =>
                {
                    await _eventLogService.SaveEventAsync(integration, 
                        _gigAuthContext.Database.CurrentTransaction);
                    await _eventLogService.MarkEventAsInProgressAsync(integration.Id);
                    _eventBus.Publish(integration);
                    await _eventLogService.MarkEventAsPublishedAsync(integration.Id);
                });
            }
            catch (Exception ex)
            {

                //_logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName} - ({@IntegrationEvent})", evt.Id, Program.AppName, evt);

                await _eventLogService.MarkEventAsFailedAsync(integration.Id);
            }
        }

        public Task PublishEvent(IntegrationEvent integration)
        {
            return Task.Run(() => _eventBus.Publish(integration));
        }
    }
}