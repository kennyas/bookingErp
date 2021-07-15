using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;
using UserManagement.Core.Enums;

namespace UserManagement.Core.Models
{
    public class BusBoy : BaseEntity
    {
        public Guid UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public GigmUser User { get; set; }

        public BusBoyStatus Status { get; set; }
    }
}