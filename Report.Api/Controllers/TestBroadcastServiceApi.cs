using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Report.Core.Hubs;
using Report.Core.Services.Interfaces;

namespace Report.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TestBroadcastServiceApi : ControllerBase
    {
        private readonly IBroadcastService _broadcastService;
        private readonly IBroadcastCustomerBookingsDashboardService _broadcastCustomerBookingsDashboardService;
        private readonly IBroadcastSalesDashboardService _broadcastSalesDashboardService;


        public TestBroadcastServiceApi(IBroadcastService broadcastService, IBroadcastCustomerBookingsDashboardService broadcastCustomerBookingsDashboardService, IBroadcastSalesDashboardService broadcastSalesDashboardService)
        {
            _broadcastService = broadcastService;
            _broadcastSalesDashboardService = broadcastSalesDashboardService;
            _broadcastCustomerBookingsDashboardService = broadcastCustomerBookingsDashboardService;
        }


        [HttpGet]
        public void BroadcastToAllConnectedClients()
        {
            _broadcastService.BroadcastToAllConnectedClients();
        }

        [HttpGet]
        public void BroadcastCurrentCustomerBookingsDashboard()
        {
            _broadcastCustomerBookingsDashboardService.CurrentCustomerBookingsDashboard();
        }

        [HttpGet]
        public void BroadcastCurrentSalesByTrip()
        {
            _broadcastSalesDashboardService.CurrentSalesByTrip();
        }
    }
}
