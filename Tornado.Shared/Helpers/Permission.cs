using System.Collections.Generic;
using System.ComponentModel;
using static Tornado.Shared.Helpers.AuthConstants;

namespace Tornado.Shared.Helpers
{
    public enum Permission
    {
        //User management Category
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        USER_MENU = 001,

        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        MANAGE_CUSTOMER = 045,
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        CUSTOMER_DETAIL = 041,
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        CUSTOMER_EDIT = 141,

        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        MANAGE_STAFF = 046, 
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        STAFF_DETAIL = 042,

        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        MANAGE_CAPTAIN = 047, 
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        CAPTAIN_DETAIL = 043,

        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        MANAGE_BUSBOY = 048,
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        BUSBOY_DETAIL = 044,

        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        MANAGE_PARTNER = 049,
        [Description(Menus.UserManagementMenu), Category(Menus.UserManagementMenu)]
        PARTNER_DETAIL = 044,


        // Role Category
        [Description(Menus.RoleMenu), Category(Menus.RoleMenu)]
        ROLE_MENU = 006,
        [Description(Menus.RoleMenu), Category(Menus.RoleMenu)]
        LIST_ROLE = 007,
        [Description(Menus.RoleMenu), Category(Menus.RoleMenu)]
        EDIT_ROLE = 008,
        [Description(Menus.RoleMenu), Category(Menus.RoleMenu)]
        DELETE_ROLE = 009,


        // Reports Category
        [Description(Menus.ReportMenu), Category(Menus.ReportMenu)]
        MANAGE_REPORT = 012,


        // Country Category
        [Description(Menus.CountryMenu), Category(Menus.CountryMenu)]
        MANAGE_COUNTRY = 013,
        [Description(Menus.CountryMenu), Category(Menus.CountryMenu)]
        LIST_COUNTRY = 014,
        [Description(Menus.CountryMenu), Category(Menus.CountryMenu)]
        EDIT_COUNTRY = 015,
        [Description(Menus.CountryMenu), Category(Menus.CountryMenu)]
        DELETE_COUNTRY = 016,
        [Description(Menus.CountryMenu), Category(Menus.CountryMenu)]
        CREATE_COUNTRY = 017,


        // Pick up Point Category
        [Description(Menus.PickupPointMenu), Category(Menus.PickupPointMenu)]
        MANAGE_PICKUPPOINT = 018,
        [Description(Menus.PickupPointMenu), Category(Menus.PickupPointMenu)]
        LIST_PICKUPPOINT = 019,
        [Description(Menus.PickupPointMenu), Category(Menus.PickupPointMenu)]
        EDIT_PICKUPPOINT = 020,
        [Description(Menus.PickupPointMenu), Category(Menus.PickupPointMenu)]
        DELETE_PICKUPPOINT = 021,
        [Description(Menus.PickupPointMenu), Category(Menus.PickupPointMenu)]
        CREATE_PICKUPPOINT = 022,


        // Route Category
        [Description(Menus.RouteMenu), Category(Menus.RouteMenu)]
        MANAGE_ROUTE = 023,
        [Description(Menus.RouteMenu), Category(Menus.RouteMenu)]
        LIST_ROUTE = 024,
        [Description(Menus.RouteMenu), Category(Menus.RouteMenu)]
        EDIT_ROUTE = 025,
        [Description(Menus.RouteMenu), Category(Menus.RouteMenu)]
        DELETE_ROUTE = 026,
        [Description(Menus.RouteMenu), Category(Menus.RouteMenu)]
        CREATE_ROUTE = 027,



        //State Category
        [Description(Menus.StateMenu), Category(Menus.StateMenu)]
        MANAGE_STATE = 028,
        [Description(Menus.StateMenu), Category(Menus.StateMenu)]
        LIST_STATE = 029,
        [Description(Menus.StateMenu), Category(Menus.StateMenu)]
        EDIT_STATE = 030,
        [Description(Menus.StateMenu), Category(Menus.StateMenu)]
        DELETE_STATE = 031,
        [Description(Menus.StateMenu), Category(Menus.StateMenu)]
        CREATE_STATE = 032,

        //Dashboard  Category
        [Description(Menus.DashboardMenu), Category(Menus.DashboardMenu)]
        DASHBOARD_SALES = 011, 
        [Description(Menus.DashboardMenu), Category(Menus.DashboardMenu)]
        DASHBOARD = 010,
    }


    public static class Menus
    {
        public const string UserManagementMenu = "User Management Menu";
        public const string StateMenu = "State Menu";
        public const string RouteMenu = "Route Menu";
        public const string ReportMenu = "Report Menu";
        public const string CountryMenu = "Country Menu";
        public const string PickupPointMenu = "PickupPoint Menu";
        public const string RoleMenu = "Role Menu";
        public const string DashboardMenu = "Dashboard Menu";



    }
    public static class PermisionProvider
    {
        public static Dictionary<string, IEnumerable<Permission>> GetSystemDefaultRoles()
        {
            return new Dictionary<string, IEnumerable<Permission>>
            {
                    {    RoleHelpers.SUPER_ADMIN, new Permission []{
                                                        Permission.DASHBOARD,
                                                        Permission.DASHBOARD_SALES,
                                                        Permission.DELETE_ROLE,
                                                        Permission.EDIT_ROLE,
                                                        Permission.LIST_ROLE,
                                                        Permission.MANAGE_REPORT,

                         }
                    },
                    {    RoleHelpers.SYS_ADMIN, new Permission []{
                                                        Permission.DASHBOARD,
                                                        Permission.DASHBOARD_SALES,
                                                        Permission.DELETE_ROLE,
                                                        Permission.EDIT_ROLE,
                                                        Permission.LIST_ROLE,
                                                        Permission.MANAGE_REPORT,
                                                        Permission.MANAGE_COUNTRY,
                                                        Permission.LIST_COUNTRY,
                                                        Permission.EDIT_COUNTRY,
                                                        Permission.DELETE_COUNTRY,
                                                        Permission.CREATE_COUNTRY,
                                                        Permission.MANAGE_PICKUPPOINT,
                                                        Permission.LIST_PICKUPPOINT ,
                                                        Permission.EDIT_PICKUPPOINT ,
                                                        Permission.DELETE_PICKUPPOINT,
                                                        Permission.CREATE_PICKUPPOINT,
                                                        Permission.MANAGE_ROUTE,
                                                        Permission.LIST_ROUTE ,
                                                        Permission.EDIT_ROUTE ,
                                                        Permission.DELETE_ROUTE,
                                                        Permission.CREATE_ROUTE,
                                                        Permission.MANAGE_STATE,
                                                        Permission.LIST_STATE ,
                                                        Permission.EDIT_STATE ,
                                                        Permission.DELETE_STATE,
                                                        Permission.CREATE_STATE,
                         }
                    },
                    {    RoleHelpers.SUPPORT, new Permission []{

                         }
                    },
                    {    RoleHelpers.PARTNER, new Permission []{

                         }
                    },
                    {    RoleHelpers.BUS_BOY, new Permission []{

                         }
                    },
                    {    RoleHelpers.CAPTAIN, new Permission []{
                         }
                    },
                    {    RoleHelpers.CUSTOMER, new Permission []{

                         }
                    },

            };
        }
    }
}