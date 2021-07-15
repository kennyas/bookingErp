using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class TripInformationMap : BaseEntityTypeConfiguration<TripManagement>
    {
        public override void Configure(EntityTypeBuilder<TripManagement> builder)
        {
            base.Configure(builder);
        }
    }
}
