using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Shared.Models.Map;

namespace Booking.Core.Models.Map
{
    public class VehicleExcludedSeatMap : BaseEntityTypeConfiguration<VehicleExcludedSeat>
    {
        public override void Configure(EntityTypeBuilder<VehicleExcludedSeat> builder)
        {
        }
    }
}