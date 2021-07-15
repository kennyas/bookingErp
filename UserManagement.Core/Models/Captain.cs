using UserManagement.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class Captain : BaseEntity
    {
        [MaxLength(100)]
        public string EmployeeCode { get; set; }
        [ForeignKey(nameof(UserId))]
        public GigmUser User { get; set; }
        public Guid UserId { get; set; }
        public CaptainStatus? Status { get; set; }
    }
}