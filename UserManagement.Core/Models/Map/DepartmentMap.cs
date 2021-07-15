using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace UserManagement.Core.Models.Map
{
    public class DepartmentMap : BaseEntityTypeConfiguration<Department>
    {
        public override void Configure(EntityTypeBuilder<Department> builder)
        {
            base.Configure(builder);
        }
    }
}
