using Report.Core.Models;
using Report.Core.ViewModels;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Report.Core.Services.Interfaces
{
    public interface ISalesDashboardService : IService<CustomerBookings>
    {
        Task<ApiResponse<TotalSalesViewModel>> GetTotalSales(GetTotalSalesViewModel model);

        Task<ApiResponse<PaginatedList<TotalSalesByTerminalViewModel>>> GetTotalSalesByTerminal(GetTotalSalesByTerminalViewModel model);

        Task<ApiResponse<PaginatedList<TotalSalesByRouteViewModel>>> GetTotalSalesByRoute(GetTotalSalesByRouteViewModel model);

        Task<ApiResponse<PaginatedList<TotalSalesByVehicleViewModel>>> GetTotalSalesByVehicle(GetTotalSalesByVehicleViewModel model);

        Task<ApiResponse<PaginatedList<TotalSalesByTripViewModel>>> GetTotalSalesByTrip(GetTotalSalesByTripViewModel model);

    }
}
