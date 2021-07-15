using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Statics;
using static Tornado.Shared.Helpers.AuthConstants;

namespace Tornado.Shared.Models.Map
{
    public class MyAppUserRoleMap : IEntityTypeConfiguration<GigmUserRole>
    {
        public void Configure(EntityTypeBuilder<GigmUserRole> builder)
        {
            builder.ToTable("GigUserRole");
            builder.HasKey(p => new { p.UserId, p.RoleId });
            SetupData(builder);
        }

        private void SetupData(EntityTypeBuilder<GigmUserRole> builder)
        {
            List<GigmUserRole> dataList = new List<GigmUserRole>()
            {
                new GigmUserRole()
                {
                    UserId = Defaults.AdminId,
                    RoleId = RoleHelpers.SUPER_ADMIN_ID(),
                }
            };

            builder.HasData(dataList);
        }
    }
}
