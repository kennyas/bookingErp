using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class VehicleModelRouteFeeViewModel
    {
        public Guid? Id { get; set; }
        public string RouteName { get; set; }

        public Guid RouteId { get; set; }
        public string VehicleModelTitle { get; set; }
        public Guid VehicleModelId { get; set; }

        public decimal BaseFare { get; set; }
    }

    public class VehicleModelRouteFeeRequestViewModel : BaseSearchViewModel
    {
        [Required]
        public Guid RouteId { get; set; }
        public Guid? VehicleModelId { get; set; }

        //public Guid? DepartureRoutePointId { get; set; }

        //public Guid? DestinationRoutePointId { get; set; }

    }

    public class CreateVehicleModelRouteFeeViewModel
    {
        [Required]
        public Guid RouteId { get; set; }
        [Required]

        public Guid VehicleModelId { get; set; }
        public decimal BaseFare { get; set; }

    }

    public class EditVehicleModelRouteFeeViewModel
    {
        [Required]
        public Guid RouteId { get; set; }
        [Required]

        public Guid VehicleModelId { get; set; }
        [Required]

        public decimal BaseFare { get; set; }
        [Required]

        public Guid Id { get; set; }
    }
}
