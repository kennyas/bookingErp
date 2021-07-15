using Report.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace Report.Core.Services
{
    public class BookingsAndSalesDashboardBroadcast : IBroadcastService
    {
        private readonly IBroadcastCustomerBookingsDashboardService _broadcastCustomerBookingsDashboardService;
        private readonly IBroadcastSalesDashboardService _broadcastSalesDashboardService;

        public BookingsAndSalesDashboardBroadcast(IBroadcastCustomerBookingsDashboardService broadcastCustomerBookingsDashboardService, IBroadcastSalesDashboardService broadcastSalesDashboardService)
        {
            _broadcastCustomerBookingsDashboardService = broadcastCustomerBookingsDashboardService;
            _broadcastSalesDashboardService = broadcastSalesDashboardService;
        }
        public async Task BroadcastToAllConnectedClients()
        {
            await Task.Run(() =>
            {
                var realtimeData = _broadcastCustomerBookingsDashboardService.GetData();
                _ = _broadcastCustomerBookingsDashboardService.LoadData(realtimeData).BroadcastAll();
                _ = _broadcastSalesDashboardService.LoadData(realtimeData).BroadcastAll();
            });
        }
    }
}
