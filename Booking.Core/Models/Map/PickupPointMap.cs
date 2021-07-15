using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Collections.Generic;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;
using Tornado.Shared.Timing;

namespace Booking.Core.Models.Map
{
    public class PickupPointMap : BaseEntityTypeConfiguration<Point>
    {
        public override void Configure(EntityTypeBuilder<Point> builder)
        {
            builder.Property(p => p.Latitude).HasColumnType("decimal(10,8)");
            builder.Property(p => p.Longitude).HasColumnType("decimal(11,8)");
            base.Configure(builder);
        }

    }
}