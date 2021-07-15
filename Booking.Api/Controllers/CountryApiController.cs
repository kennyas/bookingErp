using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Policy;
using Tornado.Shared.EF;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class CountryApiController : BaseController
    {
        private readonly ICountryService _countryService;
        public CountryApiController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost]
        [RequiresPermission(Permission.CREATE_COUNTRY)]
        public async Task<ApiResponse<CountryCreateModel>> CreateCountry(CountryCreateModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _countryService.CreateCountryAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPut]
        [RequiresPermission(Permission.EDIT_COUNTRY)]
        public async Task<ApiResponse<CountryViewModel>> EditCountry(CountryEditModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _countryService.EditCountryAsync(model).ConfigureAwait(false) ;
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        [RequiresPermission(Permission.DELETE_COUNTRY)]
        public async Task<ApiResponse<CountryViewModel>> DeleteCountry(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _countryService.DeleteCountryAsync(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        [RequiresPermission(Permission.LIST_COUNTRY)]
        public async Task<ApiResponse<CountryViewModel>> GetCountry(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _countryService.GetCountryAsync(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPost]
        [RequiresPermission(Permission.LIST_COUNTRY)]
        public async Task<ApiResponse<PaginatedList<CountryViewModel>>> GetAllCountry(BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _countryService.GetAllCountryAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<List<string>>> GetAllDialingCodes()
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _countryService.GetCountryDialingCodes().ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}