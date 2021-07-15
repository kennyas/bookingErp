using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;

namespace Booking.Core.Models.Map
{
    public class CountryMap : BaseEntityTypeConfiguration<Country>
    {
        public override void Configure(EntityTypeBuilder<Country> builder)
        {
          base.Configure(builder);
        }
    }
}