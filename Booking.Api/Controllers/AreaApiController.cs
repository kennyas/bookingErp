using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AreaApiController : BaseController
    {
        private readonly IAreaService _areaService;
        public AreaApiController(IAreaService areaService)
        {
            _areaService = areaService;
        }
        [HttpGet]
        public async Task<ApiResponse<PaginatedList<AreaListViewModel>>> GetAllAreas([FromQuery]AreaSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _areaService.GetAreas(model).ConfigureAwait(false);


            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<CreateAreaViewModel>> CreateArea(CreateAreaViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _areaService.CreateArea(model).ConfigureAwait(false);


            }).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ApiResponse<EditAreaViewModel>> EditArea(EditAreaViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _areaService.EditArea(model).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<AreaViewModel>> GetArea(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _areaService.GetArea(id).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }
        [HttpDelete]
        [AllowAnonymous]
        public async Task<ApiResponse<AreaViewModel>> DeleteArea(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _areaService.DeleteArea(id).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }
    }
}
