using Audit.Core.Models.Map;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Tornado.Shared.Context;

namespace Audit.Core.Context
{
    public class GigAuditContext : GigDbContext
    {
        public GigAuditContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.ApplyConfiguration(new AuditLogMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}