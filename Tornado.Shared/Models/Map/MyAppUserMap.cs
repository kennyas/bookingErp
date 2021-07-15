using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Tornado.Shared.Enums;
using Tornado.Shared.Statics;

namespace Tornado.Shared.Models.Map
{
    public class MyAppUserMap : IEntityTypeConfiguration<GigmUser>
    {
        public MyAppUserMap()
        {
        }

        public PasswordHasher<GigmUser> Hasher { get; set; } = new PasswordHasher<GigmUser>();

        public void Configure(EntityTypeBuilder<GigmUser> builder)
        {
            builder.ToTable("GigUser");
            builder.Property(b => b.FirstName).HasMaxLength(150);
            builder.Property(b => b.LastName).HasMaxLength(150);
            builder.Property(b => b.MiddleName).HasMaxLength(150);

            SetupSuperAdmin(builder);
            SetupAdmin(builder);
        }

        private void SetupSuperAdmin(EntityTypeBuilder<GigmUser> builder)
        {
            var sysUser = new GigmUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                FirstName = "GIG",
                LastName = "GCP",
                Id = Defaults.SysUserId,
                LastLoginDate = DateTime.Now,
                Email = Defaults.SysUserEmail,
                EmailConfirmed = true,
                Gender = Gender.MALE,
                NormalizedEmail = Defaults.SysUserEmail.ToUpper(),
                PhoneNumber = Defaults.SysUserMobile,
                UserName = Defaults.SysUserEmail,
                NormalizedUserName = Defaults.SysUserEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "micr0s0ft_"),
                SecurityStamp = "99ae0c45-d682-4542-9ba7-1281e471916b",
                UserType = UserType.STAFF,
            };

            var superUser = new GigmUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                Id = Defaults.SuperAdminId,
                FirstName = "GIG",
                LastName = "GCP",
                LastLoginDate = DateTime.Now,
                Email = Defaults.SuperAdminEmail,
                Gender = Gender.FEMALE,
                EmailConfirmed = true,
                NormalizedEmail = Defaults.SuperAdminEmail.ToUpper(),
                PhoneNumber = Defaults.SuperAdminMobile,
                UserName = Defaults.SuperAdminEmail,
                NormalizedUserName = Defaults.SuperAdminEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "123!@#QWE"),
                SecurityStamp = "016020e3-5c50-40b4-9e66-bba56c9f5bf2",
                UserType = UserType.STAFF
            };

            builder.HasData(sysUser, superUser);
        }

        private void SetupAdmin(EntityTypeBuilder<GigmUser> builder)
        {
            var adminUser = new GigmUser
            {
                Activated = true,
                CreatedOnUtc = DateTime.Now,
                FirstName = "Admin",
                LastName = "User",
                Id = Defaults.AdminId,
                LastLoginDate = DateTime.Now,
                Email = Defaults.AdminEmail,
                EmailConfirmed = true,
                Gender = Gender.UNKNOWN,
                NormalizedEmail = Defaults.AdminEmail.ToUpper(),
                PhoneNumber = Defaults.AdminMobile,
                UserName = Defaults.AdminEmail,
                NormalizedUserName = Defaults.AdminEmail.ToUpper(),
                TwoFactorEnabled = false,
                PhoneNumberConfirmed = true,
                PasswordHash = Hasher.HashPassword(null, "123!@#QWE"),
                SecurityStamp = "7d728c76-1c51-491a-99db-bde6a5b0655b",
                UserType = UserType.STAFF,
            };

            builder.HasData(adminUser);

        }

    }
}
