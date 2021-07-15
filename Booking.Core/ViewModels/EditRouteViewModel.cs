using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class EditRouteViewModel
    {
        public Guid? DeparturePickupPointId { get; set; }
        public Guid? DestinationPickupPointId { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public Guid CreatedId { get; set; }
        public Guid ModifiedBy { get; set; }
    }
}
