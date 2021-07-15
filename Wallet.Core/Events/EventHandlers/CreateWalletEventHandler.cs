using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Wallet.Core.Services.Interfaces;

namespace Wallet.Core.Events.EventHandlers
{
    public class CreateWalletEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly ICustomerWalletService _customerWalletService;
        public CreateWalletEventHandler(ICustomerWalletService customerWalletService)
        {
            _customerWalletService = customerWalletService;
        }
        public Task Handle(UserCreatedIntegrationEvent @event)
        {
            var addWallet = _customerWalletService.AddCustomerWalletAsync(new Models.CustomerWallet { });
            throw new NotImplementedException();
        }
    }
}
