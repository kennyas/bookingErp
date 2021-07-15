using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class RouteTripViewModel : BaseViewModel
    {
          public List<RouteViewModel> Routes { get; set; }
    }
}
