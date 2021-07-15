using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using Tornado.Shared.Helpers;
using Tornado.Shared.Statics;
using static Tornado.Shared.Helpers.AuthConstants;

namespace Tornado.Shared.Models.Map
{
    public class MyAppUserClaimMap : IEntityTypeConfiguration<GigmUserClaim>
    {
        private static int counter = 1;

        public void Configure(EntityTypeBuilder<GigmUserClaim> builder)
        {
            builder.ToTable("GigUserClaim");
            builder.HasKey(c => c.Id);

            builder.HasData(AdminPermissionData());
            builder.HasData(SuperAdminPermissionData());
        }

        private static IEnumerable<GigmUserClaim> AdminPermissionData()
        {
            var sysAdminPermissions = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelpers.SYS_ADMIN)
                .SelectMany(x => x.Value);


            foreach (var item in sysAdminPermissions)
            {
                yield return new GigmUserClaim
                {
                    Id = counter++,
                    ClaimType = nameof(Permission),
                    ClaimValue = item.ToString(),
                    UserId = Defaults.AdminId
                };
            }
        }

        private static IEnumerable<GigmUserClaim> SuperAdminPermissionData()
        {
            var superAdminPermission = PermisionProvider.GetSystemDefaultRoles()
                .Where(x => x.Key == RoleHelpers.SUPER_ADMIN)
                .SelectMany(x => x.Value);


            foreach (var item in superAdminPermission)
            {
                yield return new GigmUserClaim
                {
                    Id = ++counter,
                    ClaimType = nameof(Permission),
                    ClaimValue = item.ToString(),
                    UserId = Defaults.SuperAdminId
                };
            }
        }
    }
}