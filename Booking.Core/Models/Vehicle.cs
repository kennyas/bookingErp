using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Vehicle : BaseEntity
    {
        public Guid VehicleModelId { get; set; }
        [ForeignKey(nameof(VehicleModelId))]
        public VehicleModel VehicleModel { get; set; }

        public string RegistrationNumber { get; set; }

        public string ChassisNumber { get; set; }

        public Guid? PartnerId { get; set; }

        //added fields
        public VehicleStatus Status { get; set; }
    }
}
