using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class ExternalBookingDetailsConfigMap:  BaseEntityTypeConfiguration<GigBookingDetails>
    {
        public override void Configure(EntityTypeBuilder<GigBookingDetails> builder)
        {
            base.Configure(builder);
        }
    }
}
