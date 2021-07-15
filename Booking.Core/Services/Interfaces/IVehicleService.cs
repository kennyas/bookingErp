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
    public interface IVehicleService : IService<Vehicle>
    {
        Task<ApiResponse<VehicleViewModel>> CreateVehicleAsync(VehicleViewModel model);
        Task<ApiResponse<VehicleViewModel>> EditVehicleAsync(EditVehicleViewModel model);
        Task<ApiResponse<VehicleViewModel>> DeleteVehicleAsync(DeleteVehicleViewModel model);
        Task<ApiResponse<PaginatedList<VehicleViewModel>>> GetAllVehicleAsync(BasePaginatedViewModel model);
        Task<ApiResponse<VehicleViewModel>> GetVehicleByChasisNumberAsync(SearchVehicleByChassisNumberViewModel model);
        Task<ApiResponse<PaginatedList<VehicleViewModel>>> GetVehicleByPartnerIdAsync(GetVehicleByPartnerIdViewModel model);
        Task<ApiResponse<VehicleViewModel>> GetVehicleByRegistrationNumberAsync(SearchVehicleByRegistrationNumberViewModel model);
        Task<ApiResponse<PaginatedList<VehicleViewModel>>> SearchVehicleAsync(VehiclePaginatedViewModel model);
        Task<ApiResponse<List<TripVehicleViewModel>>> GetUnattachedVehicles(TripVehicleSearchModel model);
    }
}
