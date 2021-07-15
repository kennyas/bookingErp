using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class RouteHikeMap : BaseEntityTypeConfiguration<RouteHike>
    {
        public override void Configure(EntityTypeBuilder<RouteHike> builder)
        {
            base.Configure(builder);
        }
    }
}
