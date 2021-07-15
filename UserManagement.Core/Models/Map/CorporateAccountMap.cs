﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Models.Map;

namespace UserManagement.Core.Models.Map
{
    public class CorporateAccountMap : BaseEntityTypeConfiguration<CorporateAccount>
    {
        public override void Configure(EntityTypeBuilder<CorporateAccount> builder)
        {
            base.Configure(builder);
        }
    }
}
