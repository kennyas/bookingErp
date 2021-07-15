using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class SubRouteFeeViewModel
    {
        public Guid? Id { get; set; }
        public string DeparturePointName { get; set; }
        public string DestinationPointName { get; set; }

        public Guid? DepartureRoutePointId { get; set; }
        public string VehicleModelTitle { get; set; }
        public Guid VehicleModelId { get; set; }
        public Guid DeparturePointId { get; set; }

        public Guid RouteId { get; set; }

        public decimal Fare { get; set; }

    }

    public class SubRouteFeeRequestViewModel : BaseSearchViewModel
    {
        [Required]
        public Guid RouteId { get; set; }
        public Guid? VehicleModelId { get; set; }

        //public Guid? DepartureRoutePointId { get; set; }

        //public Guid? DestinationRoutePointId { get; set; }

    }

    public class CreateSubRouteFeeRequestViewModel
    {
        [Required]
        public Guid RouteId { get; set; }
        [Required]

        public Guid DepartureRoutePointId { get; set; }
        [Required]


        public decimal Fare { get; set; }

        public Guid VehicleModelId { get; set; }

    }

    public class EditSubRouteFeeRequestViewModel
    {
        [Required]
        public Guid Id { get; set; }


        [Required]
        public Guid RouteId { get; set; }
        [Required]

        public Guid DepartureRoutePointId { get; set; }
        [Required]


        public decimal Fare { get; set; }

        [Required]
        public Guid VehicleModelId { get; set; }

    }
}
