using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class Customer : BaseEntity
    {
        [ForeignKey(nameof(UserId))]
        public GigmUser User { get; set; }
        public Guid UserId { get; set; }

        public Guid? CorporateAccountId { get; set; }
        [MaxLength(500)]
        public string HomeAddress { get; set; }
        [MaxLength(500)]
        public string OfficeAddress { get; set; }

        public string DeviceToken { get; set; }

    }
}
