using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.IntegrationEventLogEF;

namespace Wallet.Core.Context.EventLog
{
    public class WalletIntegrationLogContext : IntegrationEventLogContext
    {
        public WalletIntegrationLogContext(DbContextOptions<WalletIntegrationLogContext> options) : base(options)
        {

        }
    }
}
