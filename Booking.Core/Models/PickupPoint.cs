using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Point : BaseEntity
    {
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        [MaxLength(200)]
        public string Title { get; set; }
        [MaxLength(300)]

        public string Description { get; set; }
        [MaxLength(200)]
        public string ShortDescription { get; set; }

        public Guid AreaId { get; set; }
        [ForeignKey(nameof(AreaId))]
        public Area Area{ get; set; }
    }
}
