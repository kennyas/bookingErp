using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.IntegrationEventLogEF;

namespace Booking.Core.Context
{
    public class BookingIntegrationLogContext : IntegrationEventLogContext
    {
        public BookingIntegrationLogContext(DbContextOptions<BookingIntegrationLogContext> options) : base(options)
        {

        }
    }
}
