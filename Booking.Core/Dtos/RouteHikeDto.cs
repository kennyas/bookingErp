using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace Booking.Core.Dtos
{
    public class RouteHikeDto : BaseDto
    {
        public Guid RouteId { get; set; }      
        public Guid HikeId { get; set; }
    }
}
