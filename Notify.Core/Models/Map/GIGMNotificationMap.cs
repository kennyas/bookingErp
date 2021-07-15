using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Notify.Core.Models.Map
{
    class GIGMNotificationMap : BaseEntityTypeConfiguration<GIGMNotification>
    {
        public override void Configure(EntityTypeBuilder<GIGMNotification> builder)
        {
            base.Configure(builder);
        }
    }
}
