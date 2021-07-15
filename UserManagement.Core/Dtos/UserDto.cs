using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace UserManagement.Core.Dtos
{
    public class UserDto : BaseDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string RoleName { get; set; }
        public Guid? RoleId { get; set; }
        public int Gender { get; set; }

    }
}
