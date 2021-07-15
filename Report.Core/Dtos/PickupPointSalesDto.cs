using System;
using Tornado.Shared.Dtos;

namespace Report.Core.Dtos
{
    public class PickupPointSalesDto : BaseDto
    {
        public string PickupPointName { get; set; }
        public decimal Amount { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
