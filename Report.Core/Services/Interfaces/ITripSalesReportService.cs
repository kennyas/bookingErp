using Report.Core.Models;
using Report.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Report.Core.Services.Interfaces
{
    public interface ITripSalesReportService : IService<CustomerBookings>
    {
        Task<ApiResponse<PaginatedList<TripSalesReportViewModel>>> GetTripSalesReport(GetTripSalesReportViewModel model);
    }
}
