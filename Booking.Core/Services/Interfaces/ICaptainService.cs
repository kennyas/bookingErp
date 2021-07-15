using Booking.Core.Models;
using Booking.Core.ViewModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Booking.Core.Services.Interfaces
{
    public interface ICaptainService
    {
        void Add(Captain driver);
        Task<List<ValidationResult>> IsVehicleAttachedToCaptain(VehicleAttachedViewModel model);
        Task<List<ValidationResult>> AttachedToCaptain(CaptainVehicleAttachedViewModel model);
    }
}