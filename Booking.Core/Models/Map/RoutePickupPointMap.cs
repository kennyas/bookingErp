using Booking.Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;

namespace Booking.Core.Models.Map
{
    class RoutePickupPointMap : BaseEntityTypeConfiguration<RoutePoint>
    {
        public override void Configure(EntityTypeBuilder<RoutePoint> builder)
        {

            base.Configure(builder);
        }
    }
}