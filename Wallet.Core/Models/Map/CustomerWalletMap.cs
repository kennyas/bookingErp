using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Wallet.Core.Models.Map
{

    public class CustomerWalletMap : BaseEntityTypeConfiguration<CustomerWallet>
    {
        public override void Configure(EntityTypeBuilder<CustomerWallet> builder)
        {
            base.Configure(builder);
        }
    }
}
