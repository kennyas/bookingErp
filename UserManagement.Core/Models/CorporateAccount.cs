using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class CorporateAccount : BaseEntity
    {
        [ForeignKey(nameof(UserId))]
        public GigmUser User { get; set; }
        public Guid UserId { get; set; }
        [MaxLength(150)]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
