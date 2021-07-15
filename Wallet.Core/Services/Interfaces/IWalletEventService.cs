using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Wallet.Core.Services.Interfaces
{
   public interface IWalletEventService
    {
        Task PublishEvent(IntegrationEvent integration);
    }
}
