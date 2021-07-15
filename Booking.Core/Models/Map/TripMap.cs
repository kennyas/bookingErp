using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;

namespace Booking.Core.Models.Map
{
    public class TripMap : BaseEntityTypeConfiguration<Trip>
    {
        public override void Configure(EntityTypeBuilder<Trip> builder)
        {
      
            base.Configure(builder);
        }
    }
}
