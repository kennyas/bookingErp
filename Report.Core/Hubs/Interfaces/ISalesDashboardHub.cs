using Report.Core.Enums;
using Report.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Core.Hubs.Interfaces
{
    public interface ISalesDashboardHub
    {
        Task CurrentTotalSales(TotalSalesViewModel model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients);

        Task CurrentSalesByTerminal(List<TotalSalesByTerminalViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients);

        Task CurrentSalesByRoute(List<TotalSalesByRouteViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients);

        Task CurrentTotalSalesByVehicle(List<TotalSalesByVehicleViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients);

        Task CurrentTotalSalesByTrip(List<TotalSalesByTripViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients);
    }
}
