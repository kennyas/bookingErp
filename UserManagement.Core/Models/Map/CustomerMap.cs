using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace UserManagement.Core.Models.Map
{
    public class CustomerMap : BaseEntityTypeConfiguration<Customer>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            base.Configure(builder);
        }
    }
}
