using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Helpers;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RoleMgtApiController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleMgtApiController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<ApiResponse<string>> Create([FromBody]GigmRoleCreateViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<string>(errors: "Request is invalid, Please confirm and try again.");

                var results = await _roleService.CreateAsync(model);

                if (!results.Any())
                    return new ApiResponse<string>("Role created successfully.");

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpGet]
        public async Task<ApiResponse<List<GigmRole>>> Get([FromQuery]BaseSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<List<GigmRole>>(errors: "Request is invalid, Please confirm and try again.");

                var roles = await _roleService.Get(model.Keyword, model.PageIndex, model.PageTotal, out int total);

                return new ApiResponse<List<GigmRole>>(roles, totalCount: total);
            });
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ApiResponse<GigmRoleViewModel>> Get([Required]string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var role = await _roleService.Get(id);

                return new ApiResponse<GigmRoleViewModel>(role);
            });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ApiResponse<string>> Delete([Required]string id)
        {
            return await HandleApiOperationAsync(async () =>
            {

                var results = await _roleService.DeleteRoleByIdAsync(id);

                if (!results.Any())
                    return new ApiResponse<string>("Role deleted successfully.");

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            });
        }


        [HttpPut]
        public async Task<ApiResponse<string>> Update([FromBody]GigmRoleUpdateViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<string>(errors: "Request is invalid, Please confirm and try again.");

                var results = await _roleService.UpdateAsync(model);

                if (!results.Any())
                    return new ApiResponse<string>("Role updated successfully.");

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            });
        }


        [HttpPost]
        public async Task<ApiResponse<string>> AddUserToRole([FromBody]AddUserToRoleViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (model is null)
                    return new ApiResponse<string>(errors: "Request is invalid, Please confirm and try again.");

                var results = await _roleService.AddUserToRole(model.UserId,model.RoleId);

                if (!results.Any())
                    return new ApiResponse<string>("User added to specified role successfully.");

                return new ApiResponse<string>(errors: results.Select(x => x.ErrorMessage).ToArray());
            });
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ApiResponse<PermissionViewModel[]>> GetAllPermissions()
        {
            return await HandleApiOperationAsync(async () => {

                var results = await _roleService.GetAllPermissions();

                return new ApiResponse<PermissionViewModel[]>(data: results, codes: Tornado.Shared.Enums.ApiResponseCodes.OK);
            });
        }
    }
}