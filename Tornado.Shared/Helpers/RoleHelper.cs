using System;

namespace Tornado.Shared.Helpers
{
    public static class AuthConstants
    {
        public static class RoleHelpers
        {
            public static Guid SYS_ADMIN_ID() => Guid.Parse("a1b6b6b0-0825-4975-a93d-df3dc86f8cc7");
            public const string SYS_ADMIN = nameof(SYS_ADMIN);

            public static Guid SUPER_ADMIN_ID() => Guid.Parse("0718ddef-4067-4f29-aaa1-98c1548c1807");
            public const string SUPER_ADMIN = nameof(SUPER_ADMIN);

            public static Guid SUPPORT_ID() => Guid.Parse("0718ddef-4067-4f29-aaa1-98c1548c1907");
            public const string SUPPORT = nameof(SUPPORT);

            public static Guid PARTNER_ID() => Guid.Parse("be62b371-8a25-441e-9592-f8a8ccdb7c9a");
            public const string PARTNER = nameof(PARTNER);

            public static Guid CAPTAIN_ID() => Guid.Parse("09993d49-b51a-4d35-a39f-017fafeae6f9");
            public const string CAPTAIN = nameof(CAPTAIN);

            public static Guid BUS_BOY_ID() => Guid.Parse("f950bb7e-714c-4d58-9a96-f46c7e20bab4");
            public const string BUS_BOY = nameof(BUS_BOY);

            public static Guid CUSTOMER_ID() => Guid.Parse("02bde570-4aa8-4c60-a462-07154aa69520");
            public const string CUSTOMER = nameof(CUSTOMER);

            public static Guid STAFF_ID() => Guid.Parse("3c38af54-ceca-463f-9407-06a231a3c6f6");
            public const string STAFF = nameof(STAFF);
        }

        public static class JwtClaimIdentifiers
        {
            public const string Rol = "rol", Id = "id", Jti = "Jti", Iat = "Iat", sub = "Sub";
            public const string Pol_ApiAccess_Key = "ApiUser";
        }

        //public static class JwtClaims
        //{
        //    //roles 
        //    public const string Role_Administrator = RoleHelpers.SYS_ADMIN;
        //    public const string Role_super_admin = RoleHelpers.SUPER_ADMIN;
        //    public const string Role_Support = RoleHelpers.SUPPORT;
        //    public const string Role_Corporate = RoleHelpers.CORPORATE_USER;
        //    public const string Role_bus_boy = RoleHelpers.BUS_BOY;
        //    public const string Role_bus_driver = RoleHelpers.BUS_DRIVER;
        //    public const string customer = RoleHelpers.CUSTOMER;
        //    //reference for permission access
        //    //https://docs.microsoft.com/en-us/aspnet/core/security/authorization/secure-data?view=aspnetcore-2.1

        //}
    }
}
