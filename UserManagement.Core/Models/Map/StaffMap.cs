using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace UserManagement.Core.Models.Map
{
    public class StaffMap : BaseEntityTypeConfiguration<Staff>
    {
        public override void Configure(EntityTypeBuilder<Staff> builder)
        {
            base.Configure(builder);
        }
    }
}
