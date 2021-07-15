using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Models.Map
{
    public class MyAppRoleClaimMap : IEntityTypeConfiguration<GigmRoleClaim>
    {
        public void Configure(EntityTypeBuilder<GigmRoleClaim> builder)
        {
            builder.ToTable("GigRoleClaim");
        }
    }
}
