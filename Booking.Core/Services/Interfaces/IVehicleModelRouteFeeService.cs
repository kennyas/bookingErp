using Booking.Core.Models;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface IVehicleModelRouteFeeService : IService<VehicleModelRouteFee>
    {
        Task<List<VehicleModelRouteFeeViewModel>> GetAllVehicleModelRouteFees(VehicleModelRouteFeeRequestViewModel model, out int totalCount);

        Task<List<ValidationResult>> CreateVehicleModelRouteFee(CreateVehicleModelRouteFeeViewModel model);
        Task<List<ValidationResult>> DeleteVehicleModelRouteFee(string id);
        Task<List<ValidationResult>> EditVehicleModelRouteFee(EditVehicleModelRouteFeeViewModel model);
        Task<ApiResponse<VehicleModelRouteFeeViewModel>> GetVehicleModelRouteFee(string id);
    }
}
