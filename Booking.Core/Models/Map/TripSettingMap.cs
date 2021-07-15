using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class TripSettingMap : BaseEntityTypeConfiguration<TripDays>
    {
        public override void Configure(EntityTypeBuilder<TripDays> builder)
        {
            base.Configure(builder);
        }
    }
}
