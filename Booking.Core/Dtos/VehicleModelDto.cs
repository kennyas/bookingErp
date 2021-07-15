using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace Booking.Core.Dtos
{
    public class VehicleModelDto : BaseDto
    {
        public string Title { get; set; }
        public Guid VehicleMakeId { get; set; }
        public string VehicleMake { get; set; }
        public string Description { get; set; }
        public Guid? CreatedId { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
