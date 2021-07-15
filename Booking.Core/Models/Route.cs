using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Route : BaseEntity
    {
        [MaxLength(150)]
        public string Name { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }

        [MaxLength(150)]
        public string ShortDescription { get; set; }
    }
}