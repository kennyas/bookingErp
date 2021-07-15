using Booking.Core.Dtos;
using Booking.Core.Enums;
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
    public interface IPickupPointService : IService<Point>
    {
        IEnumerable<PickUpDestinationPointDto> GetPoint(Guid ppPointId, Guid dpPointId);
        RoutePointDto GetPoint(Guid pointId, PointType pointType, Guid routeId);
        Task<ApiResponse<PickupPointViewModel>> CreatePickupPointAsync(PickupPointViewModel model);
        Task<ApiResponse<PickupPointViewModel>> EditPickupPointAsync(PickupPointViewModel model);
        Task<ApiResponse<PickupPointViewModel>> DeletePickupPointAsync(DeletePickupPointViewModel model);
        Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetAllPickupPointsAsync(PickupPointPaginatedViewModel model);
        Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointsByTitleAsync(SearchPickupPointViewModel model);
        Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointAsync(GetPickupPointByIdViewModel model);
        Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointsByStateAsync(SearchPickupPointViewModel model);
        Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDesitinationPickupPointsByDepartureAsync(GetRoutePickUpPointByDepartureId model);
        Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDeparturePickupPointsByDestinationAsync(GetDeparturePickUpPointByDestinationId model);
        Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDeparturePickupPointsAsync(GetDeparturePickUpPoints model);
        Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetOrderedDeparturePickupPointsAsync(GetOrderedDeparturePickUpPoints model);
    }
}
