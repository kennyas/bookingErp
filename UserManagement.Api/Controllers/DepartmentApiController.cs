using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DepartmentApiController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentApiController(IDepartmentService DepartmentService)
        {
            _departmentService = DepartmentService;
        }

        [HttpPost]
        public async Task<ApiResponse<DepartmentViewModel>> CreateDepartment(DepartmentViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _departmentService.CreateDepartmentAsync(model);
            });
        }

        [HttpPost]
        public async Task<ApiResponse<DepartmentViewModel>> EditDepartment(DepartmentViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _departmentService.EditDepartmentAsync(model);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<DepartmentViewModel>> DeleteDepartment(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _departmentService.DeleteDepartmentAsync(id);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<DepartmentViewModel>> GetDepartment(string id)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _departmentService.GetDepartmentAsync(id);
            });
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<DepartmentViewModel>>> GetAllDepartment([FromQuery] BaseSearchViewModel searchViewmodel)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _departmentService.GetAllDepartmentAsync(searchViewmodel);
            });
        }
    }
}