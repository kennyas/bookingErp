using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class BookingMap : BaseEntityTypeConfiguration<Booking>
    {
        public override void Configure(EntityTypeBuilder<Booking> builder)
        {

            builder.HasIndex("RefCode", "BookingPhoneNumber");
            base.Configure(builder);
        }
    }
}

