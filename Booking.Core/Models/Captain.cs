using Booking.Core.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Tornado.Shared.Models;

namespace Booking.Core.Models
{
    public class Captain : BaseEntity
    {
        //[MaxLength(100)]
        //public string EmployeeCode { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public Guid UserId { get; set; }
        public CaptainStatus CaptainStatus { get; set; }
        public Guid? AttachedVehicleId { get; set; }
        public Vehicle AttachedVehicle { get; set; }
    }
}