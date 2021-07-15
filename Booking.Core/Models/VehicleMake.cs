using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class VehicleMake : BaseEntity
    {
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(300)]
        public string Description { get; set; }
        [MaxLength(150)]
        public string ShortDescription { get; set; }
    }
}
