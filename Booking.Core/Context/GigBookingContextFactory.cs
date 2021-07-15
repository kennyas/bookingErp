using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using Tornado.Shared.Context;

namespace Booking.Core.Context
{
    public class GigBookingContextFactory : GigDbContextFactory<GigBookingContext>
    {
    }


}
