using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;

namespace Booking.Core.Services
{
    public class ExternalBookingDetailsService : Service<GigBookingDetails>, IExternalBookingDetailsService
    {
        public ExternalBookingDetailsService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
