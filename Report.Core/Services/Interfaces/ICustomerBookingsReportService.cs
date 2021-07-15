using Report.Core.Models;
using Report.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Report.Core.Services.Interfaces
{
    public interface ICustomerBookingsReportService : IService<CustomerBookings>
    {
        Task<ApiResponse<PaginatedList<CustomerBookingsReportViewModel>>> GetCustomerBookings(GetCustomerBookingsReportViewModel model);

        Task<ApiResponse<CustomerBookingsReportViewModel>> AddCustomerBookings(AddCustomerBookingsReportViewModel model);
        Task<ApiResponse<List<CustomerBookingsReportViewModel>>> AddCustomerBookings(List<AddCustomerBookingsReportViewModel> model);
        Task<ApiResponse<CustomerBookingsReportViewModel>> UpdateCustomerBookings(UpdateCustomerBookingsReportViewModel model);
    }
}
