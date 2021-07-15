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
    public interface ISubRouteFeeService : IService<SubRouteFee>
    {
        Task<List<SubRouteFeeViewModel>> GetAllSubRouteFees(SubRouteFeeRequestViewModel model, out int totalCount);

        Task<List<ValidationResult>> CreateSubRouteFee(CreateSubRouteFeeRequestViewModel model);
        Task<List<ValidationResult>> DeleteSubRouteFee(string id);
        Task<List<ValidationResult>> EditSubRouteFee(EditSubRouteFeeRequestViewModel model);
        Task<ApiResponse<SubRouteFeeViewModel>> GetSubRouteFee(string id);
    }
}
