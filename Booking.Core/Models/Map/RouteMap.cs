using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;
using Tornado.Shared.Timing;

namespace Booking.Core.Models.Map
{
    public class RouteMap : BaseEntityTypeConfiguration<Route>
    {
        public override void Configure(EntityTypeBuilder<Route> builder)
        {
           
            base.Configure(builder);
        }
    }
}