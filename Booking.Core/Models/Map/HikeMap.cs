using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class HikeMap : BaseEntityTypeConfiguration<Hike>
    {
        public override void Configure(EntityTypeBuilder<Hike> builder)
        {
            base.Configure(builder);
        }
    }
}
