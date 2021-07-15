using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Models.Map
{
    public class MyAppUserTokenMap : IEntityTypeConfiguration<GigmUserToken>
    {
        public void Configure(EntityTypeBuilder<GigmUserToken> builder)
        {
            //builder.HasKey(p => p.UserId);
            builder.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });
            builder.ToTable("GigUserToken");

        }
    }
}
