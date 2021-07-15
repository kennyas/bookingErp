using System;
using System.Collections.Generic;
using System.Text;

namespace Tornado.Shared.ViewModels
{
    public class UserPermissionActionViewModel
    {
        public Guid UserId { get; set; }
        public string[] Permissions { get; set; }
    }

    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }

        public string DisplayName { get; set; }


    }
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
