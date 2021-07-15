using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class VehicleModel : BaseEntity
    {
        [MaxLength(150)]
        public string Title { get; set; }
        [MaxLength(150)]
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public Guid VehicleMakeId { get; set; }
        [ForeignKey(nameof(VehicleMakeId))]
        public VehicleMake VehicleMake { get; set; }

        public int NoOfSeats { get; set; }

    }
}
