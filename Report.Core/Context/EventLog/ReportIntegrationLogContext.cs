using Microsoft.EntityFrameworkCore;
using Tornado.Shared.IntegrationEventLogEF;

namespace Report.Core.Context
{
    public class ReportIntegrationLogContext : IntegrationEventLogContext
    {
        public ReportIntegrationLogContext(DbContextOptions<ReportIntegrationLogContext> options) : base(options)
        {

        }
    }
}
