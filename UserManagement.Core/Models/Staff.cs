using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class Staff : BaseEntity
    {
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public GigmUser User { get; set; }

        public Guid DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department Department { get; set; }
        public string EmployeeCode { get;  set; }
    }
}
