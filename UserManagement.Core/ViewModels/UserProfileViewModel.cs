using System;
using Tornado.Shared.Enums;

namespace UserManagement.Core.ViewModels
{
    public class UserProfileViewModel
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string JoinDate { get; set; }
        public string LastLoginDate { get; set; }
        public bool Activated { get; set; }
        public string RoleName { get; set; }
        public Guid UserId { get; set; }
        public Gender? Gender { get; set; }
        public string Picture { get; set; }
        public bool HasPin { get; set; }
    }
}
