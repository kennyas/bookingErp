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
    public interface IVehicleMakeService : IService<VehicleMake>
    {
        Task<ApiResponse<VehicleMakeViewModel>> CreateVehicleMakeAsync(VehicleMakeViewModel model);
        Task<ApiResponse<EditVehicleMakeViewModel>> EditVehicleMakeAsync(EditVehicleMakeViewModel model);
        Task<ApiResponse<VehicleMakeViewModel>> DeleteVehicleMakeAsync(DeleteVehicleMakeViewModel model);
        Task<ApiResponse<PaginatedList<VehicleMakeViewModel>>> GetAllVehicleMakeAsync(BasePaginatedViewModel model);
        Task<ApiResponse<VehicleMakeViewModel>> GetVehicleMakeAsync(GetVehicleMakeByIdViewModel model);
        Task<ApiResponse<PaginatedList<VehicleMakeViewModel>>> SearchVehicleMakeAsync(VehicleMakeSearchViewModel model);
    }
}
