using Booking.Core.Models;
using Booking.Core.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Booking.Core.Services.Interfaces
{
    public interface IBusBoyService
    {
        Task<List<ValidationResult>> IsVehicleAttachedToBusboy(VehicleAttachedViewModel model);
        Task<List<ValidationResult>> AttachVehicleToBusBoy(BusBoyVehicleAttachedViewModel model);
        void Add(BusBoy busboy);
    }
}