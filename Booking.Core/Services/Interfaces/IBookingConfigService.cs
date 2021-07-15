using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface IBookingConfigService
    {
        Task<BookingConfigViewModel> GetConfigSettingByName(string name);
        Task<BookingConfigViewModel> CreateConfigSetting(CreateBookingConfigViewModel model);
        Task<EditBookingConfigViewModel> EditConfigSetting(EditBookingConfigRequestViewModel model);
        Task<PaginatedList<BookingConfigViewModel>> GetAllConfig(BaseSearchViewModel model);
    }
}
