using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Models.Map
{
    public class MyAppUserLoginMap : IEntityTypeConfiguration<GigmUserLogin>
    {
        public void Configure(EntityTypeBuilder<GigmUserLogin> builder)
        {
            builder.ToTable("GigUserLogin");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).ValueGeneratedOnAdd();
            builder.HasKey(u => new { u.LoginProvider, u.ProviderKey });

        }
    }
}
