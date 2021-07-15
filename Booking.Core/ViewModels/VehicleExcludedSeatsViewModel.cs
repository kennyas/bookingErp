using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class VehicleExcludedSeatsViewModel
    {
        public int Count { get; set; }

        public List<SeatDetail> SeatDetail { get; set; }
    }

    public class AddVehicleExcludedSeatsRequestViewModel
    {
        public List<SeatDetail> SeatDetails { get; set; }
        public Guid VehicleModelId { get; set; }
    }
   
    public class SeatDetail
    {
        public int SeatNumber { get; set; }

        public bool IsActive { get; set; }
    }
}
