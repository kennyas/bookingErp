
using Microsoft.AspNetCore.SignalR;
using Report.Core.Enums;
using Report.Core.Hubs.Interfaces;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System.Threading.Tasks;

namespace Report.Core.Hubs
{
    public class CustomerBookingsDashboardHub : Hub<ICustomerBookingsDashboardHub>
    {
        private readonly IBroadcastCustomerBookingsDashboardService _broadcastCustomerBookingsDashboardService;

        public CustomerBookingsDashboardHub(IBroadcastCustomerBookingsDashboardService broadcastCustomerBookingsDashboardService)
        {
            _broadcastCustomerBookingsDashboardService = broadcastCustomerBookingsDashboardService;
        }
        public Task CurrentCustomerBookingsDashboardWithData(CustomerBookingsDashboardWithDataViewModel model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients)
        {
            return connectionType == DashboardHubBroadcastType.AllConnectedClients
                   ?
               Clients.All.CurrentCustomerBookingsDashboardWithData(model)
               :
                Clients.Caller.CurrentCustomerBookingsDashboardWithData(model, connectionType)
               ;
        }

        public override Task OnConnectedAsync()
        {
            return CurrentCustomerBookingsDashboardWithData(_broadcastCustomerBookingsDashboardService.GetCurrentCustomerBookingsDashboard(), DashboardHubBroadcastType.SpecificClient);
        }
    }
}
