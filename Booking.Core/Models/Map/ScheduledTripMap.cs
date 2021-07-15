using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class ScheduledTripMap : BaseEntityTypeConfiguration<ScheduledTrip>
    {
        public override void Configure(EntityTypeBuilder<ScheduledTrip> builder)
        {
            base.Configure(builder);
        }
    }
}
