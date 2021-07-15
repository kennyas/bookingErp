using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace Audit.Core.Models.Map
{
    public class AuditLogMap : BaseEntityTypeConfiguration<AuditLog>
    {
        public override void Configure(EntityTypeBuilder<AuditLog> builder)
        {
            base.Configure(builder);
        }
    }
}
