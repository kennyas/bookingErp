using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Policy;
using Tornado.Shared.Enums;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Events;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]

    public class PartnerApiController : BaseController
    {
        private readonly IHttpUserService _currentUserService;
        private readonly IUserMnagementEventService _userEventService;
        private readonly IUserService _userService;

        public PartnerApiController(IUserService userService,
            IUserMnagementEventService userEventService,
            IHttpUserService currentUserService
            )
        {
            _currentUserService = currentUserService;
            _userService = userService;
            _userEventService = userEventService;
        }

        [HttpPost]
        //[RequiresPermission(Permission.CREATE_PARTNER)]
        public async Task<ApiResponse<SetupUserViewModel>> CreatePartner(SetupPartnerViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<SetupUserViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.CreatePartner(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserCreatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, model.Password, (int)UserType.PARTNER,
                    null, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<SetupUserViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPut]
        //[RequiresPermission(Permission.EDIT_PARTNER)]
        public async Task<ApiResponse<EditPartnerViewModel>> UpdatePartner(EditPartnerViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<EditPartnerViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.UpdatePartnerUser(model);

                return new ApiResponse<EditPartnerViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpDelete]
        //[RequiresPermission(Permission.DELETE_PARTNER)]
        public async Task<ApiResponse<string>> Delete([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.DeletePartner(userId);
                return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpGet]
        //[RequiresPermission(Permission.LIST_PARTNER)]
        public async Task<ApiResponse<dynamic>> GetPartners([FromQuery] BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<dynamic>(errors: "Request is invalid, Please confirm and try again.");

                var result = await _userService.GetAllPartner(model, out int total);

                return new ApiResponse<dynamic>(result, totalCount: total);
            });
        }


    }
}