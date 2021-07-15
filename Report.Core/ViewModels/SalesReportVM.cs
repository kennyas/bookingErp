using Report.Core.Models;
using System;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;

namespace Report.Core.ViewModels
{
    public class GetTripSalesReportViewModel : BasePaginatedViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RouteId { get; set; }
    }

    public class TripSalesReportViewModel
    {
        public string CustomerName { get; set; }
        public string TripName { get; set; }
        public decimal Amount { get; set; }
        public DateTime? DepatureDate { get; set; }
        public string DepatureTime { get; set; }
        public Guid RouteId { get; set; }

        public static explicit operator TripSalesReportViewModel(CustomerBookings source)
        {
            return new TripSalesReportViewModel
            {
                Amount = source.Amount,
                CustomerName = source.CustomerName,
                DepatureTime = source.DepatureTime,
                TripName = source.TripName,
                RouteId = source.RouteId,
                DepatureDate = source.DepartureDate
            };
        }
    }

    public class GetBusboySalesReportViewModel : BasePaginatedViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string BusboyId { get; set; }
    }

    public class BusboySalesReportViewModel
    {
        public string CustomerName { get; set; }
        public string BusboyName { get; set; }
        public decimal Amount { get; set; }
        public string RefCode { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? DepatureDate { get; set; }
        public string BookingStatus { get; set; }


        public static explicit operator BusboySalesReportViewModel(CustomerBookings source)
        {
            return new BusboySalesReportViewModel
            {
                CustomerName = source.CustomerName,
                Amount = source.Amount,
                BusboyName = source.BusboyUsername,
                RefCode = source.RefCode,
                BookingDate = source.BookingDate,
                DepatureDate = source.DepartureDate,
                BookingStatus = source.BookingStatus
            };
        }
    }
}
