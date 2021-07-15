using Booking.Core.Context;
using Booking.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Data.Common;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;
using Tornado.Shared.IntegrationEventLogEF.Services;
using Tornado.Shared.IntegrationEventLogEF.Utilities;

namespace Booking.Core.Services
{
    public class BookingEventService: IBookingEventService
    {
        private readonly IEventBus _eventBus;
        private readonly IIntegrationEventLogService _eventLogService;
        private readonly Func<DbConnection, IIntegrationEventLogService> _integrationEventLogServiceFactory;

        private readonly GigBookingContext _gigBookingContext;

        public BookingEventService(IEventBus eventBus, GigBookingContext gigBookingContext,
            Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _eventBus = eventBus;
            _gigBookingContext = gigBookingContext ?? throw new ArgumentNullException(nameof(gigBookingContext));
            _integrationEventLogServiceFactory = integrationEventLogServiceFactory;
            _eventLogService = _integrationEventLogServiceFactory(_gigBookingContext.Database.GetDbConnection());

        }

        public Task PublishEvent(IntegrationEvent integration)
        {
            return Task.Run(() => _eventBus.Publish(integration));
        }

        public async Task PublishAndLogEvent(IntegrationEvent integration)
        {
            try
            {
                await ResilientTransaction.New(_gigBookingContext).ExecuteAsync(async () =>
                {
                    await _eventLogService.SaveEventAsync(integration, _gigBookingContext.Database.CurrentTransaction);
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
    }
}