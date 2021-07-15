using System;

namespace Booking.Core.Dtos
{
    public class RoutePointDto
    {
        public Guid Id { get; set; }
        public Guid PointId { get; set; }
        public string Title { get; set; }
        public decimal? Longitude { get; set; }
        public decimal? Latitude { get; set; }
        public int OrderNo { get; set; }
       
    }

    public class PickUpDestinationPointDto {

        public Guid PickUpPointId { get; set; }
        public Guid DestinationPointId { get; set; }
        public Guid RouteId { get; set; }
        public int PickupPointOrderNo { get; set; }
        public int DestinationPointOrderNo { get; set; }
    }
}
