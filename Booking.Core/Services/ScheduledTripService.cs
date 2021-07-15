using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;

namespace Booking.Core.Services
{
    public class ScheduledTripService : Service<ScheduledTrip>, IScheduledTripService
    {
        public ScheduledTripService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
