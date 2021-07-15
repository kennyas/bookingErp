using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class State : BaseEntity
    {
        [MaxLength(100)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        [MaxLength(10)]
        public string Code { get; set; }
        public Guid CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }

    }
}
