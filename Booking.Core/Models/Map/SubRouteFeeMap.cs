using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class SubRouteFeeMap : BaseEntityTypeConfiguration<SubRouteFee>
    {
        public override void Configure(EntityTypeBuilder<SubRouteFee> builder)
        {

            base.Configure(builder);
        }
    }
}
