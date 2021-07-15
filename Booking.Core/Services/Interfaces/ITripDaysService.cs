using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface ITripDaysService
    {
        Task<ApiResponse<TripDaysViewModel>> CreateTripDays(TripDaysRequestModel model);
        Task<ApiResponse<List<TripDaysDetailViewModel>>> AllTripDays(TripDaysSearchViewModel model);
    }
}
