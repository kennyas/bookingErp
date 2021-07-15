using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Wallet.Core.Models.Map
{
    class WalletHistoryMap : BaseEntityTypeConfiguration<WalletHistory>
    {
        public override void Configure(EntityTypeBuilder<WalletHistory> builder)
        {
            base.Configure(builder);
        }
    }
}