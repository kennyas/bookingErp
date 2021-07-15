using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class RoutePoint : BaseEntity
    {
        public Guid PointId { get; set; }

        [ForeignKey(nameof(PointId))]
        public Point Point { get; set; }

        public Guid RouteId { get; set; }
        [ForeignKey(nameof(RouteId))]
        public Route Route { get; set; }

        public int OrderNo { get; set; }

        public PointType PointType { get; set; }
    }
}