using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class RouteHike : BaseEntity
    {
        public Guid RouteId { get; set; }
        [ForeignKey(nameof(RouteId))]
        public virtual Route Route { get; set; }
        public Guid HikeId { get; set;   }
        [ForeignKey(nameof(HikeId))]
        public virtual Hike Hike { get; set; }

        public bool IsActive { get; set; }
    }
}