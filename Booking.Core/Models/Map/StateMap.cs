using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;
using Tornado.Shared.Timing;

namespace Booking.Core.Models.Map
{
    public class StateMap : BaseEntityTypeConfiguration<State>
    {
        public override void Configure(EntityTypeBuilder<State> builder)
        {      

            base.Configure(builder);
        }
    }
}
