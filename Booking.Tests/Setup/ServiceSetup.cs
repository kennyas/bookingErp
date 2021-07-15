using Booking.Core.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Repository;
using Tornado.Shared.Test.Fakes;

namespace Booking.Tests.Setup
{
    public class ServiceSetup
    {
        public ServiceSetup()
        {
            var options = new DbContextOptionsBuilder<GigBookingContext>()
                        .UseSqlite("DataSource=:memory:")
                        .Options;
             Context = new GigBookingContext(options);
             UnitOfWork = new UnitOfWork(Context);


            Context.Database.OpenConnection();
            Context.Database.EnsureCreated();



        }
        public GigBookingContext Context { get; set; }
        public IUnitOfWork UnitOfWork { get; set; }
        public IHttpUserService _currentUserService { get; set; }
        public void Dispose()
        {
            Context.Database.EnsureDeleted();
        }
    }
}
