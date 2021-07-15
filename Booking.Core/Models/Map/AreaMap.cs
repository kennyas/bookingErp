using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;
using Tornado.Shared.Timing;

namespace Booking.Core.Models.Map
{
    public class AreaMap : BaseEntityTypeConfiguration<Area>
    {
        public override void Configure(EntityTypeBuilder<Area> builder)
        {
            base.Configure(builder);
        }

    }
}
