using Booking.Core.Models;
using Booking.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface ITripService : IService<Trip>
    {
        Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetAllTrip(GetAllTripViewModel model);
        Task<ApiResponse<ViewTripViewModel>> GetTripById(GetTripByIdViewModel model);
        Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsWithActiveDiscount(GetTripsWithActiveDiscountViewModel model);
        Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsByVehicleId(GetTripsByVehicleIdViewModel model);

        Task<ApiResponse<TripViewModel>> CreateTrip(TripViewModel model);

        Task<ApiResponse<ViewTripViewModel>> EditTrip(EditTripViewModel model);
        Task<ApiResponse<ViewTripViewModel>> EditTripDiscount(TripDiscountViewModel model);

        Task<ApiResponse<ViewTripViewModel>> DeleteTrip(DeleteTripViewModel model);
        Task<ApiResponse<ViewTripViewModel>> DeleteTripChildrenFee(DeleteTripChildrenFeeViewModel model);
        Task<ApiResponse<ViewTripViewModel>> DeleteTripDiscount(DeleteTripDiscountViewModel model);
        Task<List<TripSearchResponseViewModel>> SearchTrips(TripQueryViewModel searchModel,out int totalCount);
        Task<List<TripSearchResponseViewModel>> SearchTripForBusBoy(TripQueryViewModel searchModel, out int totalCount);
        Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsByRegistrationNumber(GetTripsByVehicleRegNoViewModel model);
        Task<List<BusBoySearchTripsResponseViewModel>> GetAllTodaysBusBoyTrips(out int totalCount);
    }
}