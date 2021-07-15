using Booking.Core.Models;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface IRouteService : IService<Route>
    {
        Task<ApiResponse<CreateRouteViewModel>> AddRoute(CreateRouteViewModel model);
        Task<ApiResponse<RouteViewModel>> GetRoute(string id);
        Task<ApiResponse<RouteViewModel>> EditRoute(EditRouteViewModel model);
        Task<ApiResponse<PaginatedList<RouteListViewModel>>> GetAllRoutesAsync(BaseSearchViewModel searchModel);
    }
}
