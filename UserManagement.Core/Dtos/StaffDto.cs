using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace UserManagement.Core.Dtos
{
    public class StaffDto : BaseDto
    {
        public int Gender { get; set; }
        public string DepartmentTitle { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public string FullName { get; set; }
        public string RoleName { get; set; }

    }
}
