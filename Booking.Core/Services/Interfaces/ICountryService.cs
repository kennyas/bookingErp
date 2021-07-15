using Booking.Core.Models;
using Booking.Core.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services.Interfaces
{
    public interface ICountryService : IService<Country>
    {
        Task<ApiResponse<CountryCreateModel>> CreateCountryAsync(CountryCreateModel model); 
        Task<ApiResponse<CountryViewModel>> EditCountryAsync(CountryEditModel model);
        Task<ApiResponse<CountryViewModel>> DeleteCountryAsync(string id);  
        Task<ApiResponse<CountryViewModel>> GetCountryAsync(string id);
        Task<ApiResponse<PaginatedList<CountryViewModel>>> GetAllCountryAsync(BaseSearchViewModel searchModel);

        Task<ApiResponse<List<string>>> GetCountryDialingCodes();
    }
}
