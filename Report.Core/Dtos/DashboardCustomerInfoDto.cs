using System;
using Tornado.Shared.Dtos;

namespace Report.Core.Dtos
{
    public class DashboardCustomerInfoDto : BaseDto
    {
        public string CustomerName { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public string DeparturePickupPoint { get; set; }
        public string DestinationPickupPoint { get; set; }
        public string RefCode { get; set; }
    }
}
