using Report.Core.Models;
using Report.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF.Services;

namespace Report.Core.Services.Interfaces
{
    public interface IBroadcastSalesDashboardService : IService<CustomerBookings>, IBroadcastDashboardService<BroadcastSalesDashboardService, CustomerBookings>
    {
        List<TotalSalesByTripViewModel> GetCurrentSalesByTrip(List<CustomerBookings> data = null);
        Task CurrentTotalSales();

        List<TotalSalesByVehicleViewModel> GetCurrentSalesByVehicle(List<CustomerBookings> data = null);

        TotalSalesViewModel GetCurrentTotalSales(List<CustomerBookings> data = null);

        List<TotalSalesByRouteViewModel> GetCurrentTotalSalesByRoute(List<CustomerBookings> data = null);

        List<TotalSalesByTerminalViewModel> GetCurrentTotalSalesByTerminal(List<CustomerBookings> data = null);

        Task CurrentTotalSalesByTerminal();

        Task CurrentTotalSalesByRoute();

        Task CurrentSalesByVehicle();

        Task CurrentSalesByTrip();
    }
}
