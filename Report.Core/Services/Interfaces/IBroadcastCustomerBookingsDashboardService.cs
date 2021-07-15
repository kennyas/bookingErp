using Report.Core.Models;
using Report.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF.Services;

namespace Report.Core.Services.Interfaces
{
    public interface IBroadcastCustomerBookingsDashboardService : IService<CustomerBookings>, IBroadcastDashboardService<BroadcastCustomerBookingsDashboardService, CustomerBookings>
    {
        Task CurrentCustomerBookingsDashboard();

        CustomerBookingsDashboardWithDataViewModel GetCurrentCustomerBookingsDashboard(List<CustomerBookings> data = null);
    }
}
