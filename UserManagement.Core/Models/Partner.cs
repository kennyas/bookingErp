using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Tornado.Shared.Models;

namespace UserManagement.Core.Models
{
    public class Partner : BaseEntity
    {
        [MaxLength(100)]
        public string PartnerEmail { get; set; }
        [MaxLength(50)]

        public string PartnerPhoneNumber { get; set; }
        public string PartnerAddress { get; set; }


        [ForeignKey(nameof(UserId))]
        public GigmUser User { get; set; }
        public Guid UserId { get; set; }
    }
}
