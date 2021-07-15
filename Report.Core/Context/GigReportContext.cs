using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tornado.Shared.Context;

namespace Report.Core.Context
{
    public class GigReportContext : GigDbContext
    {
        public GigReportContext(DbContextOptions<GigReportContext> options) : base(options)
        {
        }
        protected GigReportContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
