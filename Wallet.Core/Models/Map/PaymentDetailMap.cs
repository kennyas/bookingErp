using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Wallet.Core.Models.Map
{

    public class PaymentDetailMap : BaseEntityTypeConfiguration<PaymentDetail>
    {
        public override void Configure(EntityTypeBuilder<PaymentDetail> builder)
        {
            builder.HasIndex(p => p.Reference).IsUnique();
            base.Configure(builder);
        }
    }
}
