using Booking.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class RoutePickupPointSearchViewModel : BaseSearchViewModel
    {
        public Guid? DeparturePickupPointId { get; set; }
        public Guid? DestinationPickupPointId { get; set; }

    }

    public class RoutePickupPointViewModel : BaseViewModel
    {
        public Guid PickupPointId { get; set; }

        public Guid RouteId { get; set; }

        public int OrderNo { get; set; }

        public PointType PickupPointType { get; set; }

        public decimal? Longitude { get; set; }

        public decimal? Latitude { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public Guid AreaId { get; set; }
    }

    public class CreateRoutePickupPointViewModel
    {
        [Required]
        public Guid PickupPointId { get; set; }

        [Required]
        public Guid RouteId { get; set; }

        [Required]
        public int OrderNo { get; set; }

        [Required]
        public PointType PickupPointType { get; set; }
    }

    public class GetRoutePickupPoints : BasePaginatedViewModel
    {
        [Required]
        public string RouteId { get; set; }
    }   
    
    public class DeleteRoutePickupPointsById
    {
        public string RouteId { get; set; }
        public string PickupPointId { get; set; }
    }   
    
    public class DeleteAllRoutePickupPointsById
    {
        public string RouteId { get; set; }
    }

    public class GetRoutePickUpPointByDepartureId
    {
        public string DeparturePickupPointId { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
    public class GetDeparturePickUpPointByDestinationId
    {
        public string DestinationPickupPointId { get; set; }

        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }

    public class GetDeparturePickUpPoints
    {
        public Guid? StateId { get; set; }

        //public decimal Longitude { get; set; }
        //public decimal Latitude { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }

    public class GetOrderedDeparturePickUpPoints
    {
        public string Keyword { get; set; }

        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
    }
}
