﻿using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class TripRouteSearchViewModel : BaseSearchViewModel
    {
        public Guid? TripId { get; set; }
    }
}
