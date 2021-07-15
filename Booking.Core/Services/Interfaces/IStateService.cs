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
    public interface IStateService : IService<State>
    {
        Task<ApiResponse<StateViewModel>> CreateState(StateCreateModel model);

        Task<ApiResponse<StateViewModel>> EditStateAsync(StateEditModel model);
        Task<ApiResponse<StateViewModel>> DeleteStateAsync(string id);
        Task<ApiResponse<StateViewModel>> GetStateAsync(string id);
        Task<ApiResponse<PaginatedList<StateViewModel>>> GetAllStateAsync(BaseSearchViewModel searchModel); 
    }
}
