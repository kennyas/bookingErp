using Report.Core.Models;
using System;
using System.Collections.Generic;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;

namespace Report.Core.ViewModels
{
    public class GetTotalSalesViewModel
    {
        public string Date { get; set; }
    }

    public class TotalSalesViewModel
    {
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class GetTotalSalesByTerminalViewModel : BasePaginatedViewModel
    {
        public List<Guid> DepartureTerminalId { get; set; }
        public string Date { get; set; }
    }

    public class TotalSalesByTerminalViewModel
    {
        public Guid DepartureTerminalId { get; set; }
        public string DepartureTerminalName { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class GetTotalSalesByRouteViewModel : BasePaginatedViewModel
    {
        public List<Guid> RouteId { get; set; }
        public string Date { get; set; }
    }

    public class TotalSalesByRouteViewModel
    {
        public Guid RouteId { get; set; }
        public string RouteName { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class GetTotalSalesByVehicleViewModel : BasePaginatedViewModel
    {
        public List<Guid> VehicleId { get; set; }
        public string Date { get; set; }
    }

    public class TotalSalesByVehicleViewModel
    {
        public Guid VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime Date { get; set; }
    }

    public class GetTotalSalesByTripViewModel : BasePaginatedViewModel
    {
        public List<Guid> TripId { get; set; }
        public string Date { get; set; }
    }

    public class TotalSalesByTripViewModel
    {
        public Guid TripId { get; set; }
        public string TripName { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime? DepatureDate { get; set; }
    }

    public class SalesDashboardViewModel
    {
        public DateTime BookingDate { get; set; }
        public Guid DepartureTerminalId { get; set; }
        public string DepartureTerminalName { get; set; }
        public decimal Amount { get; set; }
        public Guid RouteId { get; set; }
        public string RouteName { get; set; }
        public Guid VehicleId { get; set; }
        public string RegistrationNumber { get; set; }
        public Guid TripId { get; set; }
        public string TripName { get; set; }

        public static explicit operator SalesDashboardViewModel(CustomerBookings source)
        {
            return new SalesDashboardViewModel
            {
                VehicleId = source.VehicleId,
                BookingDate = source.BookingDate,
                Amount = source.Amount,
                DepartureTerminalId = source.DepartureTerminalId,
                DepartureTerminalName = source.DepartureTerminalName,
                RegistrationNumber = source.VehicleRegistrationNumber,
                RouteId = source.RouteId,
                RouteName = source.RouteName,
                TripId = source.TripId,
                TripName = source.TripName
            };
        }
    }
}
