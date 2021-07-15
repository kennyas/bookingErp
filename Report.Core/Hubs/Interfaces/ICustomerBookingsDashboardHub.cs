using Report.Core.Enums;
using Report.Core.ViewModels;
using System.Threading.Tasks;

namespace Report.Core.Hubs.Interfaces
{
    public interface ICustomerBookingsDashboardHub
    {
        Task CurrentCustomerBookingsDashboardWithData(CustomerBookingsDashboardWithDataViewModel model, DashboardHubBroadcastType connectionType = DashboardHubBroadcastType.AllConnectedClients);
    }
}
