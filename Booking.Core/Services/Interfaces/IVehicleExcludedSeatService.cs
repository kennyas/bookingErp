using Booking.Core.Models;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface IVehicleExcludedSeatService : IService<VehicleExcludedSeat>
    {
        int[] GetExcludedSeats(Guid vehicleModelId);
        Task<ApiResponse<VehicleExcludedSeatsViewModel>> GetVehicleExcludedSeat(string vehicleModelId);
        Task<List<ValidationResult>> UpdateVehicleExcludedSeats(AddVehicleExcludedSeatsRequestViewModel model);
    }
}