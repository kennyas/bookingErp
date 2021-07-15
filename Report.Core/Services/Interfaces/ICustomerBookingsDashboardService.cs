﻿using Report.Core.Models;
using Report.Core.ViewModels;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Report.Core.Services.Interfaces
{
    public interface ICustomerBookingsDashboardService : IService<CustomerBookings>
    {
        Task<ApiResponse<CustomerBookingsDashboardWithDataViewModel>> GetCustomerBookingsDashboardByDate(GetCustomerBookingsDashboardByDateViewModel model);
    }
}
