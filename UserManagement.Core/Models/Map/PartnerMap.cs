using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace UserManagement.Core.Models.Map
{
    public class PartnerMap : BaseEntityTypeConfiguration<Partner>
    {
        public override void Configure(EntityTypeBuilder<Partner> builder)
        {
            //SetupData(builder);
            base.Configure(builder);
        }
    }
}
