
using Microsoft.AspNetCore.SignalR;
using Report.Core.Enums;
using Report.Core.Hubs.Interfaces;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Report.Core.Hubs
{
    public class SalesDashboardHub : Hub<ISalesDashboardHub>
    {
        private readonly IBroadcastSalesDashboardService _salesDashboardService;

        public SalesDashboardHub(IBroadcastSalesDashboardService salesDashboardService)
        {
            _salesDashboardService = salesDashboardService;
        }

        public Task CurrentSalesByRoute(List<TotalSalesByRouteViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients)
        {
            return connectionType == DashboardHubBroadcastType.AllConnectedClients
                                        ?
                                        Clients.All.CurrentSalesByRoute(model)
                                        :
                                        Clients.Caller.CurrentSalesByRoute(model, connectionType);
        }

        public Task CurrentSalesByTerminal(List<TotalSalesByTerminalViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients)
        {
            return connectionType == DashboardHubBroadcastType.AllConnectedClients
                                       ?
                                       Clients.All.CurrentSalesByTerminal(model)
                                       :
                                       Clients.Caller.CurrentSalesByTerminal(model, connectionType);
        }

        public Task CurrentTotalSales(TotalSalesViewModel model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients)
        {

            return connectionType == DashboardHubBroadcastType.AllConnectedClients
                                      ?
                                      Clients.All.CurrentTotalSales(model)
                                      :
                                      Clients.Caller.CurrentTotalSales(model, connectionType);
        }

        public Task CurrentTotalSalesByTrip(List<TotalSalesByTripViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients)
        {
            return connectionType == DashboardHubBroadcastType.AllConnectedClients
                                      ?
                                      Clients.All.CurrentTotalSalesByTrip(model, connectionType)
                                      :
                                      Clients.Caller.CurrentTotalSalesByTrip(model, connectionType);
        }

        public Task CurrentTotalSalesByVehicle(List<TotalSalesByVehicleViewModel> model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients)
        {
            return connectionType == DashboardHubBroadcastType.AllConnectedClients
                               ?
                               Clients.All.CurrentTotalSalesByVehicle(model)
                               :
                               Clients.Caller.CurrentTotalSalesByVehicle(model);
        }

        public override Task OnConnectedAsync()
        {
            var data = _salesDashboardService.GetData();
            var connectionType = DashboardHubBroadcastType.SpecificClient;

            return Task.Run(() =>
            {
                CurrentTotalSalesByTrip(_salesDashboardService.GetCurrentSalesByTrip(data), connectionType);
                CurrentTotalSales(_salesDashboardService.GetCurrentTotalSales(data), connectionType);
                CurrentSalesByTerminal(_salesDashboardService.GetCurrentTotalSalesByTerminal(data), connectionType);
                CurrentSalesByRoute(_salesDashboardService.GetCurrentTotalSalesByRoute(data), connectionType);
                CurrentTotalSalesByVehicle(_salesDashboardService.GetCurrentSalesByVehicle(data), connectionType);
            });
        }
    }
}
