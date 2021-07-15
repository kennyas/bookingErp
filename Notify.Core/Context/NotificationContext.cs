using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Tornado.Shared.Context;

namespace Notify.Core.Context
{
    public class NotificationContext : GigDbContext
    {
        public NotificationContext(DbContextOptions<NotificationContext> options) : base(options)
        {
        }

        protected NotificationContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
