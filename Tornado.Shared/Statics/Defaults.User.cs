using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.Statics
{
    public partial class Defaults
    {
        public const string SysUserEmail = "system@gigm.com";
        public static readonly Guid SysUserId = Guid.Parse("1989883f-4f99-43bf-a754-239bbbfec00e");
        public const string SysUserMobile = "08062066851";

        public const string SuperAdminEmail = "superadmin@gigm.com";
        public static readonly Guid SuperAdminId = Guid.Parse("3fb897c8-c25d-4328-9813-cb1544369fba");
        public const string SuperAdminMobile = "08062066851";

        public static Guid AdminId = Guid.Parse("973AF7A9-7F18-4E8B-ACD3-BC906580561A");
        public const string AdminEmail = "admin@gigm.com";
        public const string AdminMobile = "08062066851";

        public static readonly Guid CustomerUserId = Guid.Parse("129712e3-9214-4dd3-9c03-cfc4eb9ba979");
        public const string CustomerUserMobile = "08062066851";
        public const string CustomerUserEmail = "basic@gigm.com";

        public static readonly Guid PartnerUserId = Guid.Parse("129712e3-9214-4dd3-9c03-cfc4eb9ba979");
        public const string PartnerUserMobile = "08062066851";
        public const string PartnerUserEmail = "partner@google.com";

        public static readonly Guid CorporateUserId = Guid.Parse("129712e3-9214-4dd3-9c03-cfc4eb9ba979");
        public const string CorporateUserMobile = "08062066851";
        public const string CorporateUserEmail = "partner@stellas.com";
    }
}
