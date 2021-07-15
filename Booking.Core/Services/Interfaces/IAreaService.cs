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
    public interface IAreaService : IService<Area>
    {
        Task<ApiResponse<PaginatedList<AreaListViewModel>>> GetAreas(AreaSearchViewModel model);
        Task<ApiResponse<CreateAreaViewModel>> CreateArea(CreateAreaViewModel model);
        Task<ApiResponse<EditAreaViewModel>> EditArea(EditAreaViewModel model);
        Task<ApiResponse<AreaViewModel>> GetArea(string id);
        Task<ApiResponse<AreaViewModel>> DeleteArea(string id);
    }
}
