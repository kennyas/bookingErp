using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Wallet.Core.Models.Map
{
    public class CardDetailsMap : BaseEntityTypeConfiguration<CardDetails>
    {
        public override void Configure(EntityTypeBuilder<CardDetails> builder)
        {
            base.Configure(builder);
        }
    }
}
