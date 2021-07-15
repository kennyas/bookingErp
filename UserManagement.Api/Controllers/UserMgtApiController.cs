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
    public class UserMgtApiController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IHttpUserService _currentUserService;
        private readonly IUserMnagementEventService _userEventService;

        public UserMgtApiController(
            IUserService userService,
            IUserMnagementEventService userEventService,
            IHttpUserService currentUserService
            )
        {
            _currentUserService = currentUserService;
            _userService = userService;
            _userEventService = userEventService;
        }

        [HttpGet]
        [RequiresPermission(Permission.MANAGE_STAFF)]
        public async Task<ApiResponse<dynamic>> GetStaffs([FromQuery] BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<dynamic>(errors: "Request is invalid, Please confirm and try again.");

                var result = await _userService.GetAllStaff(model, out int total);

                return new ApiResponse<dynamic>(result, totalCount: total);
            });
        }

        [HttpGet]
        [RequiresPermission(Permission.MANAGE_CUSTOMER)]
        public async Task<ApiResponse<dynamic>> GetCustomers([FromQuery] BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<dynamic>(errors: "Request is invalid, Please confirm and try again.");

                var result = await _userService.GetAllCustomer(model, out int total);

                return new ApiResponse<dynamic>(result, totalCount: total);
            });
        }

        [HttpGet]
        [RequiresPermission(Permission.MANAGE_PARTNER)]
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

        [HttpGet]
        [RequiresPermission(Permission.MANAGE_CAPTAIN)]
        public async Task<ApiResponse<dynamic>> GetCaptains([FromQuery] BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<dynamic>(errors: "Request is invalid, Please confirm and try again.");

                var result = await _userService.GetAllCaptain(model, out int total);

                return new ApiResponse<dynamic>(result, totalCount: total);
            });
        }

        [HttpGet]
        [RequiresPermission(Permission.MANAGE_BUSBOY)]
        public async Task<ApiResponse<dynamic>> GetBusBoys([FromQuery] BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<dynamic>(errors: "Request is invalid, Please confirm and try again.");

                var result = await _userService.GetAllBusBoy(model, out int total);

                return new ApiResponse<dynamic>(result, totalCount: total);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<UserViewModel>> GetCustomer([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.GetCustomerById(userId);

                return new ApiResponse<UserViewModel>(result);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<BusBoyViewModel>> GetBusBoy([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.GetBusBoyById(userId);

                return new ApiResponse<BusBoyViewModel>(result);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<StaffViewModel>> GetStaff([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.GetStaffById(userId);

                return new ApiResponse<StaffViewModel>(result);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<PartnerViewModel>> GetPartner([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.GetPartnerById(userId);

                return new ApiResponse<PartnerViewModel>(result);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<CaptainViewModel>> GetCaptain([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.GetCaptainById(userId);

                return new ApiResponse<CaptainViewModel>(result);
            });
        }

        [HttpPut]
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

        [HttpPut]
        public async Task<ApiResponse<EditStaffViewModel>> UpdateStaff(EditStaffViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<EditStaffViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.UpdateStaffUser(model);

                return new ApiResponse<EditStaffViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPut]
        public async Task<ApiResponse<EditStaffViewModel>> UpdateBusBoy(EditBusBoyViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<EditStaffViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.UpdateBusBoyUser(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserUpdatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, (int)UserType.BUSBOY,
                    null, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<EditStaffViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPut]
        public async Task<ApiResponse<EditCaptainViewModel>> UpdateCaptain(EditCaptainViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<EditCaptainViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.UpdateCaptainUser(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserUpdatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, (int)UserType.CAPTAIN,
                    null, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<EditCaptainViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPut]
        public async Task<ApiResponse<EditStaffViewModel>> UpdateCustomer(EditUserViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<EditStaffViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.UpdateCustomerUser(model);

                return new ApiResponse<EditStaffViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpDelete]
        public async Task<ApiResponse<string>> DeleteStaff([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.DeleteStaff(userId);
                return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpDelete]
        public async Task<ApiResponse<string>> DeletePartner([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.DeletePartner(userId);
                return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpDelete]
        public async Task<ApiResponse<string>> DeleteCustomer([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.DeleteCustomer(userId);
                return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpDelete]
        public async Task<ApiResponse<string>> DeleteCaptain([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.DeleteCaptain(userId);
                return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpDelete]
        public async Task<ApiResponse<string>> DeleteBusBoy([Required] string userId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.DeleteBusBoy(userId);
                return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPost]
        public async Task<ApiResponse<SetupUserViewModel>> CreateStaff(SetupStaffViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<SetupUserViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.CreateStaff(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserCreatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, model.Password, (int)UserType.STAFF,
                    model.EmployeeCode, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<SetupUserViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPost]
        public async Task<ApiResponse<SetupBusBoyViewModel>> CreateBusBoy(SetupBusBoyViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<SetupBusBoyViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.CreateBusBoy(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserCreatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, model.Password, (int)UserType.BUSBOY,
                    null, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<SetupBusBoyViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPost]
        public async Task<ApiResponse<SetupCaptainViewModel>> CreateCaptain(SetupCaptainViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<SetupCaptainViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.CreateCaptain(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserCreatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, model.Password, (int)UserType.CAPTAIN,
                    model.EmployeeCode, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<SetupCaptainViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpPost]
        public async Task<ApiResponse<SetupPartnerViewModel>> CreatePartner(SetupPartnerViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<SetupPartnerViewModel>(errors: ListModelErrors.ToArray());

                var result = await _userService.CreatePartner(model);

                if (!result.Any())
                {
                    await _userEventService.PublishAndLogEvent(new UserCreatedIntegrationEvent(
                    model.FirstName, model.LastName, model.Email,
                    model.Email, model.Password, (int)UserType.PARTNER,
                    null, model.Id,
                    _currentUserService.GetCurrentUser().UserId));
                }

                return new ApiResponse<SetupPartnerViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray());
            });
        }
    }
}