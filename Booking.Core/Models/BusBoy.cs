using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class BusBoy : BaseEntity
    {
        public Guid UserId { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }

        public BusBoyStatus BusBoyStatus { get; set; }

        public Guid? AttachedVehicleId { get; set; }
        public Vehicle AttachedVehicle { get; set; }

    }
}