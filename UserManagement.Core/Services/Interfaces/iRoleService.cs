using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Tornado.Shared.Helpers;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services.Interfaces
{
    public interface IRoleService
    {
        Task<List<ValidationResult>> CreateAsync(GigmRoleCreateViewModel model);
        Task<List<ValidationResult>> DeleteRoleByIdAsync(string id);
        Task<GigmRole> FindByNameAsync(string name);
        Task<List<GigmRole>> Get(string name, int? page, int? size, out int totalCount);
        Task<List<Claim>> GetClaimsByRoleIdAsync(string id);
        Task<GigmRoleViewModel> Get(string id);
        Task<List<ValidationResult>> UpdateAsync(GigmRoleUpdateViewModel model);
        Task<List<ValidationResult>> AddUserToRole(string userId, string roleId);

        Task<PermissionViewModel[]> GetAllPermissions();
    }
}