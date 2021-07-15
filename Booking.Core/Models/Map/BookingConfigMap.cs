using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class BookingConfigMap :  BaseEntityTypeConfiguration<BookingConfig>
    {
        public override void Configure(EntityTypeBuilder<BookingConfig> builder)
        {
            base.Configure(builder);
        }
    }
}
