using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Enums;
using static Tornado.Shared.Helpers.AuthConstants;

namespace Tornado.Shared.Models.Map
{
    public class MyAppRoleMap : IEntityTypeConfiguration<GigmRole>
    {
        public void Configure(EntityTypeBuilder<GigmRole> builder)
        {
            builder.ToTable("GigRole");
            SetupData(builder);
        }


        private void SetupData(EntityTypeBuilder<GigmRole> builder)
        {
            var roles = new GigmRole[]
            {
                new GigmRole
                {
                    Id = RoleHelpers.SYS_ADMIN_ID(),
                    Name = UserRole.SYS_ADMIN.ToString(),
                    NormalizedName = UserRole.SYS_ADMIN.ToString()
                },
                new GigmRole
                {
                    Id = RoleHelpers.SUPER_ADMIN_ID(),
                    Name = UserRole.SUPER_ADMIN.ToString(),
                    NormalizedName = UserRole.SUPER_ADMIN.ToString()
                },
                 new GigmRole
                {
                    Id = RoleHelpers.CUSTOMER_ID(),
                    Name = UserRole.CUSTOMER.ToString(),
                    NormalizedName = UserRole.CUSTOMER.ToString()
                },
                new GigmRole
                {
                    Id = RoleHelpers.BUS_BOY_ID(),
                    Name = UserRole.BUS_BOY.ToString(),
                    NormalizedName = UserRole.BUS_BOY.ToString()
                },
                 new GigmRole
                {
                    Id = RoleHelpers.PARTNER_ID(),
                    Name = UserRole.PARTNER.ToString(),
                    NormalizedName = UserRole.PARTNER.ToString()
                },
                  new GigmRole
                {
                    Id = RoleHelpers.CAPTAIN_ID(),
                    Name = UserRole.DRIVER.ToString(),
                    NormalizedName = UserRole.DRIVER.ToString()
                },
                  new GigmRole
                {
                    Id = RoleHelpers.STAFF_ID(),
                    Name = UserRole.STAFF.ToString(),
                    NormalizedName = UserRole.STAFF.ToString()
                },
            };

            builder.HasData(roles);
        }

    }
}