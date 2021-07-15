using Tornado.Shared.Helpers;

namespace UserManagement.Core.ViewModels
{
    public class GigmRoleCreateViewModel
    {
        public string Name { get; set; }
        public Permission[] Permissions { get; set; }
    }

    public class GigmRoleUpdateViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Permission[] Permissions { get; set; }
    }

    public class GigmRoleViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public Permission[] Permissions { get; set; }
    }

    public class AddUserToRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}