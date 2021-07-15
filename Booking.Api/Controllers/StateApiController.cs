using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Policy;
using Tornado.Shared.EF;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class StateApiController : BaseController
    {
        private readonly IStateService _stateService;

        public StateApiController(IStateService stateService)
        {
            _stateService = stateService;
        }

        [HttpPost]
        [RequiresPermission(Permission.CREATE_STATE)]
        public async Task<ApiResponse<StateViewModel>> CreateState(StateCreateModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _stateService.CreateState(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPut]
        [RequiresPermission(Permission.EDIT_STATE)]
        public async Task<ApiResponse<StateViewModel>> EditState(StateEditModel model) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _stateService.EditStateAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        [RequiresPermission(Permission.DELETE_STATE)]
        public async Task<ApiResponse<StateViewModel>> DeleteState(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _stateService.DeleteStateAsync(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        //[RequiresPermission(Permission.LIST_STATE)]
        [AllowAnonymous]
        public async Task<ApiResponse<StateViewModel>> GetState(string id) 
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _stateService.GetStateAsync(id).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPost]
        [RequiresPermission(Permission.LIST_STATE)]
        public async Task<ApiResponse<PaginatedList<StateViewModel>>> GetAllState([FromBody]BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _stateService.GetAllStateAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
