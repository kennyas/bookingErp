
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Report.Core.Enums;
using Report.Core.Hubs;
using Report.Core.Hubs.Interfaces;
using Report.Core.Models;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Timing;
using Tornado.Shared.Utils;

namespace Report.Core.Services
{
    public class BroadcastSalesDashboardService : Service<CustomerBookings>, IBroadcastSalesDashboardService
    {
        private readonly IHubContext<SalesDashboardHub, ISalesDashboardHub> _salesDashboardHub;
        public List<CustomerBookings> Data { get; set; }

        public BroadcastSalesDashboardService(IUnitOfWork unitOfWork, IHubContext<SalesDashboardHub, ISalesDashboardHub> salesDashboardHub) : base(unitOfWork)
        {
            _salesDashboardHub = salesDashboardHub;
        }

        public async Task BroadcastAll()
        {
            await Task.Run(() =>
                {
                    _ = CurrentSalesByTrip();
                    _ = CurrentSalesByVehicle();
                    _ = CurrentTotalSales();
                    _ = CurrentTotalSalesByRoute();
                    _ = CurrentTotalSalesByTerminal();
                });
        }

        public List<TotalSalesByTripViewModel> GetCurrentSalesByTrip(List<CustomerBookings> data = null)
        {
            string paymentApproved = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID)?.ToLower();

            if (data != null)
                Data = data;

            if (Data == null)
                LoadData();

            return (from dataModel in Data
                    where dataModel.PaymentStatus.Equals(paymentApproved, StringComparison.InvariantCultureIgnoreCase)
                    orderby dataModel.BookingDate ascending
                    group dataModel by new { dataModel.TripId, dataModel.TripName } into d
                    select new TotalSalesByTripViewModel
                    {
                        TripId = d.Key.TripId,
                        TotalAmount = d.Sum(d => d.Amount),
                        TripName = d.Key.TripName
                    }).ToList();
        }

        public async Task CurrentSalesByTrip()
        {
            await _salesDashboardHub.Clients.All.CurrentTotalSalesByTrip(GetCurrentSalesByTrip());
        }

        public List<TotalSalesByVehicleViewModel> GetCurrentSalesByVehicle(List<CustomerBookings> data = null)
        {
            if (data != null)
                Data = data;

            if (Data == null)
                LoadData();

            string paymentApproved = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID)?.ToLower();

            DateTime currentDate = DateTime.Now.GetStartOfDay();

            return (from dataModel in Data
                    where dataModel.PaymentStatus.Equals(paymentApproved, StringComparison.InvariantCultureIgnoreCase)
                    orderby dataModel.BookingDate ascending
                    group dataModel by new
                    {
                        dataModel.VehicleId,
                        dataModel.VehicleRegistrationNumber,
                    } into d
                    select new TotalSalesByVehicleViewModel
                    {
                        VehicleId = d.Key.VehicleId,
                        TotalAmount = d.Sum(d => d.Amount),
                        RegistrationNumber = d.Key.VehicleRegistrationNumber,
                        Date = currentDate
                    }).ToList();
        }

        public async Task CurrentSalesByVehicle()
        {
            await _salesDashboardHub.Clients.All.CurrentTotalSalesByVehicle(GetCurrentSalesByVehicle());
        }

        public TotalSalesViewModel GetCurrentTotalSales(List<CustomerBookings> data = null)
        {
            if (data != null)
                Data = data;

            if (Data == null)
                LoadData();

            string paymentApproved = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID)?.ToLower();

            return new TotalSalesViewModel
            {
                Date = Clock.Now.GetStartOfDay(),
                Amount = Data.Where(d => d.PaymentStatus.Equals(paymentApproved, StringComparison.InvariantCultureIgnoreCase)).Sum(data => data.Amount)
            };
        }

        public async Task CurrentTotalSales()
        {
            await _salesDashboardHub.Clients.All.CurrentTotalSales(GetCurrentTotalSales());
        }

        public List<TotalSalesByRouteViewModel> GetCurrentTotalSalesByRoute(List<CustomerBookings> data = null)
        {
            if (data != null)
                Data = data;

            if (Data == null)
                LoadData();


            string paymentApproved = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID)?.ToLower();

            return (from dataModel in Data
                    where dataModel.PaymentStatus.Equals(paymentApproved, StringComparison.InvariantCultureIgnoreCase)
                    orderby dataModel.BookingDate ascending
                    group dataModel by new { dataModel.RouteId, dataModel.RouteName } into d
                    select new TotalSalesByRouteViewModel
                    {
                        RouteId = d.Key.RouteId,
                        TotalAmount = d.Sum(d => d.Amount),
                        RouteName = d.Key.RouteName
                    }).ToList();
        }

        public async Task CurrentTotalSalesByRoute()
        {
            await _salesDashboardHub.Clients.All.CurrentSalesByRoute(GetCurrentTotalSalesByRoute());
        }

        public List<TotalSalesByTerminalViewModel> GetCurrentTotalSalesByTerminal(List<CustomerBookings> data = null)
        {
            if (data != null)
                Data = data;

            if (Data == null)
                LoadData();

            string paymentApproved = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID)?.ToLower();

            return (from dataModel in Data
                    where dataModel.PaymentStatus.Equals(paymentApproved, StringComparison.InvariantCultureIgnoreCase)
                    group dataModel by new { dataModel.DepartureTerminalId, dataModel.DepartureTerminalName } into d
                    select new TotalSalesByTerminalViewModel
                    {
                        DepartureTerminalId = d.Key.DepartureTerminalId,
                        TotalAmount = d.Sum(d => d.Amount),
                        DepartureTerminalName = d.Key.DepartureTerminalName
                    }).ToList();
        }

        public async Task CurrentTotalSalesByTerminal()
        {
            await _salesDashboardHub.Clients.All.CurrentSalesByTerminal(GetCurrentTotalSalesByTerminal());
        }

        public BroadcastSalesDashboardService LoadData()
        {
            DateTime startDate = DateTime.Now.GetStartOfDay();
            DateTime endDate = startDate.GetEndOfDay();

            Data = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate).Select(d => d).ToList();

            return this;
        }

        public BroadcastSalesDashboardService LoadData(List<CustomerBookings> dataSource)
        {
            Data = dataSource;
            return this;
        }

        public List<CustomerBookings> GetData()
        {
            DateTime startDate = DateTime.Now.GetStartOfDay();
            DateTime endDate = startDate.GetEndOfDay();

            return UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().Where(d => d.BookingDate >= startDate && d.BookingDate <=  endDate).Select(d => d).ToList();
        }

    }
}
