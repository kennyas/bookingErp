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
    public interface IRouteHikeService : IService<RouteHike> 
    {
        Task<ApiResponse<RouteHikeCreateViewModel>> CreateRouteHikeAsync(RouteHikeCreateViewModel model);
        Task<ApiResponse<RouteHikeEditViewModel>> EditRouteHikeAsync(RouteHikeEditViewModel model); 
        Task<ApiResponse<RouteHikeEditViewModel>> DeleteRouteHikeAsync(string id); 
        Task<ApiResponse<RouteHikeCreateViewModel>> GetRouteHikeAsync(string id); 
       // Task<ApiResponse<PaginatedList<RouteHikeEditViewModel>>> GetAllRouteHikeAsync(CountrySearchVModel searchModel); 
    }
}
