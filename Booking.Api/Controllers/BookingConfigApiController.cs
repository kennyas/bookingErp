using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.Enums;
using Tornado.Shared.ViewModels;
using System.Linq;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class BookingConfigApiController : BaseController
    {
        private readonly IBookingConfigService _bookingConfigService;
        public BookingConfigApiController(IBookingConfigService bookingConfigService)
        {
            _bookingConfigService = bookingConfigService;
        }
        [HttpGet]
        public async Task<ApiResponse<BookingConfigViewModel>> GetConfiguration(string name)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var configData = await _bookingConfigService.GetConfigSettingByName(name).ConfigureAwait(false);

                return new ApiResponse<BookingConfigViewModel>(data: configData, message: configData == null ? "Config data does not exist" : "", codes: configData == null? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK);
            }).ConfigureAwait(false); ;
        }

        [HttpPost]

        public async Task<ApiResponse<BookingConfigViewModel>> CreateConfuguration(CreateBookingConfigViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var configData = await _bookingConfigService.CreateConfigSetting(model).ConfigureAwait(false);

                return new ApiResponse<BookingConfigViewModel>(data: configData, message: configData == null ? "Config was not created" : "", codes: configData == null ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK);
            }).ConfigureAwait(false); ;
        }

        
        [HttpPut]
        public async Task<ApiResponse<EditBookingConfigViewModel>> EditConfuguration(EditBookingConfigRequestViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var configData = await _bookingConfigService.EditConfigSetting(model).ConfigureAwait(false);

                return new ApiResponse<EditBookingConfigViewModel>(data: configData, message: configData == null ? "Config was not created" : "", codes: configData == null ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK);
            }).ConfigureAwait(false); ;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ApiResponse<PaginatedList<BookingConfigViewModel>>> GetAllConfig(BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var configData = await _bookingConfigService.GetAllConfig(model).ConfigureAwait(false);

                return new ApiResponse<PaginatedList<BookingConfigViewModel>>(data: configData, 
                    message: configData.Any() ? "Empty list" : "Successful", 
                    codes: configData.Any() ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK);
            }).ConfigureAwait(false); ;
        }

    }
}