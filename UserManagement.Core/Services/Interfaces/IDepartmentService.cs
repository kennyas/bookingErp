using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Models;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services.Interfaces
{
    public interface IDepartmentService : IService<Department>
    {
        Task<ApiResponse<DepartmentViewModel>> CreateDepartmentAsync(DepartmentViewModel model);
        Task<ApiResponse<DepartmentViewModel>> EditDepartmentAsync(DepartmentViewModel model);
        Task<ApiResponse<DepartmentViewModel>> DeleteDepartmentAsync(string id);
        Task<ApiResponse<DepartmentViewModel>> GetDepartmentAsync(string id);
        Task<ApiResponse<PaginatedList<DepartmentViewModel>>> GetAllDepartmentAsync(BaseSearchViewModel searchModel);
    }
}
