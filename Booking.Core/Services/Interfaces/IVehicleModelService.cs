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
    public interface IVehicleModelService : IService<VehicleModel>
    {
        public Task<ApiResponse<VehicleModelViewModel>> CreateVehicleModelAsync(VehicleModelViewModel model);
        public Task<ApiResponse<VehicleModelViewModel>> DeleteVehicleModelAsync(DeleteVehicleModel model);
        public Task<ApiResponse<PaginatedList<VehicleModelViewModel>>> GetAllVehicleModelPaginatedAsync(VehicleModelSearch searchModel);
        public Task<ApiResponse<VehicleModelViewModel>> GetVehicleModelAsync(VehicleModelById model);
        public Task<ApiResponse<PaginatedList<VehicleModelViewModel>>> GetVehicleModelByMakeAsync(VehicleModelByMake model);
        public Task<ApiResponse<EditVehicleModelViewModel>> EditVehicleModelAsync(EditVehicleModelViewModel model);
    }
}
