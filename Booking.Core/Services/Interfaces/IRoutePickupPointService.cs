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
    public interface IRoutePickupPointService : IService<RoutePoint>
    {
        Task<ApiResponse<PaginatedList<RoutePickupPointViewModel>>> GetRoutePickupPoints(GetRoutePickupPoints model);
        Task<ApiResponse<RoutePickupPointViewModel>> AddPickupPointToRoute(CreateRoutePickupPointViewModel model);
        Task<ApiResponse<RoutePickupPointViewModel>> RemovePickupPointToRoute(DeleteRoutePickupPointsById model);
        Task<ApiResponse<RoutePickupPointViewModel>> RemoveAllRoutePickupPoints(DeleteAllRoutePickupPointsById model);
    }
}
