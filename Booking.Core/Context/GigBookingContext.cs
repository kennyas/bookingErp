using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Booking.Core.Models.Map;
using Microsoft.EntityFrameworkCore;
using Tornado.Shared.Context;

namespace Booking.Core.Context
{
    public class GigBookingContext : GigDbContext
    {
        public GigBookingContext(DbContextOptions<GigBookingContext> options) : base(options)
        {
        }
        protected GigBookingContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
