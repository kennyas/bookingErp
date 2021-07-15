using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;
using Wallet.Core.Services.Interfaces;

namespace Wallet.Core.Services
{
    public class WalletEventService : IWalletEventService
    {
        private readonly IEventBus _eventBus;

        public WalletEventService(IEventBus eventBus) 
        {
            _eventBus = eventBus;
        }

        public Task PublishEvent(IntegrationEvent integration)
        {
            return Task.Run(() => _eventBus.Publish(integration));
        }
    }
}
