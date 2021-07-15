using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    class VehicleModelRouteFeeMap : BaseEntityTypeConfiguration<VehicleModelRouteFee>
    {
        public override void Configure(EntityTypeBuilder<VehicleModelRouteFee> builder)
        {
            base.Configure(builder);
        }
    }
}