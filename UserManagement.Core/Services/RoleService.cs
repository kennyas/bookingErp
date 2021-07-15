using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tornado.Shared.Helpers;
using Tornado.Shared.Models;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<GigmRole> _roleManager;
        private readonly UserManager<GigmUser> _userManager;
        private readonly List<ValidationResult> results = new List<ValidationResult>();

        public RoleService(RoleManager<GigmRole> roleManager,
           UserManager<GigmUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public virtual Task<GigmRole> FindByNameAsync(string name)
        {
            return _roleManager.FindByNameAsync(name);
        }

        public virtual Task<GigmRole> FindByIdAsync(string id)
        {
            return _roleManager.FindByIdAsync(id);
        }

        public async Task<List<ValidationResult>> AddClaimToRoleAsync(GigmRole role, Claim claim)
        {
            var addResult = await _roleManager.AddClaimAsync(role, claim);

            if (!addResult.Succeeded)
                results.AddRange(addResult.Errors.Select(x => new ValidationResult(x.Description)));

            return results;
        }

        public async Task<List<ValidationResult>> RemoveClaimFromRoleAsync(GigmRole role, Claim claim)
        {
            var result = new List<ValidationResult>();

            var addResult = await _roleManager.RemoveClaimAsync(role, claim);

            if (!addResult.Succeeded)
                result.AddRange(addResult.Errors.Select(x => new ValidationResult(x.Description)));

            return result;
        }

        public async Task<List<ValidationResult>> DeleteRoleAsync(GigmRole role)
        {
            var deleteResult = await _roleManager.DeleteAsync(role);

            if (!deleteResult.Succeeded)
                results.AddRange(deleteResult.Errors.Select(x => new ValidationResult(x.Description)));

            return results;
        }

        public async Task<List<ValidationResult>> DeleteRoleByNameAsync(string name)
        {
            var role = await FindByNameAsync(name);

            if (role is null)
            {
                results.Add(new ValidationResult("Role NotFound"));
                return results;
            }

            return await DeleteRoleAsync(role);
        }

        public async Task<List<ValidationResult>> DeleteRoleByIdAsync(string id)
        {
            var role = await FindByIdAsync(id);

            if (role is null)
            {
                results.Add(new ValidationResult("Role NotFound"));
                return results;
            }

            return await DeleteRoleAsync(role);
        }

        public virtual async Task<List<ValidationResult>> CreateAsync(GigmRoleCreateViewModel model)
        {
            var dbrole = await FindByNameAsync(model.Name);

            if (dbrole != null)
            {
                results.Add(new ValidationResult("Role exists"));
                return results;
            }

            dbrole = new GigmRole
            {
                Name = model.Name,
            };

            var createResult = await _roleManager.CreateAsync(dbrole);

            if (!createResult.Succeeded)
            {
                results.AddRange(createResult.Errors.Select(x => new ValidationResult(x.Description)));
                return results;
            }

            var systemPermissions = Enum.GetValues(typeof(Permission)).Cast<Permission>();

            if (model.Permissions.Any())
            {
                foreach (var item in model.Permissions)
                {
                    if (systemPermissions.Contains(item))
                    {
                        var response = await AddClaimToRoleAsync(dbrole, new Claim(nameof(Permission), item.ToString()));
                        if (response.Any())
                        {
                            await DeleteRoleAsync(dbrole);
                            return response;
                        }
                    }
                }
            }
            return results;
        }

        public virtual async Task<List<Claim>> GetRoleClaimsAsync(GigmRole role)
        {
            var result = await _roleManager.GetClaimsAsync(role);

            return result.ToList();
        }

        public virtual async Task<List<Claim>> GetClaimsByRoleNameAsync(string name)
        {
            var role = await FindByNameAsync(name);

            if (role is null)
                throw new Exception("Role NotFound");

            return await GetRoleClaimsAsync(role);
        }

        public virtual async Task<List<Claim>> GetClaimsByRoleIdAsync(string id)
        {
            var role = await FindByIdAsync(id);

            if (role is null)
                throw new Exception("Role NotFound");

            return await GetRoleClaimsAsync(role);
        }

        public Task<List<GigmRole>> Get(string name, int? page, int? size, out int totalCount)
        {
            int pageIndex = (!page.HasValue || page < 1) ? 1 : page.Value;
            int pageSize = (!size.HasValue || size < 1) ? 10 : size.Value;

            var query = from r in _roleManager.Roles
                        where string.IsNullOrWhiteSpace(name) || r.Name.Contains(name)
                        orderby r.CreatedOnUtc descending

                        select r;

            totalCount = query.Count();

            return query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
        }

        public async Task<GigmRoleViewModel> Get(string id)
        {
            var role = await FindByIdAsync(id);

            if (role is null)
                throw new Exception("Role NotFound");

            var permissions = await GetRoleClaimsAsync(role);

            return new GigmRoleViewModel
            {
                Id = role.Id.ToString(),
                Name = role.Name,
                Permissions = ClaimsToPermissions(permissions)
            };
        }

        private Permission[] ClaimsToPermissions(List<Claim> claims)
        {
            var list = new List<Permission>();

            foreach (Claim c in claims.Where(x => x.Type == nameof(Permission)))
                list.Add(Enum.Parse<Permission>(c.Value));

            return list.ToArray();
        }

        public async Task<List<ValidationResult>> AddUserToRole(string userId, string roleId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var userRoles = await _userManager.GetRolesAsync(user);

            await _userManager.RemoveFromRolesAsync(user, userRoles);

            var role = await FindByIdAsync(roleId);

            await _userManager.AddToRoleAsync(user, role.Name);

            return results;
        }

        public async Task<List<ValidationResult>> UpdateAsync(GigmRoleUpdateViewModel model)
        {
            var role = await FindByIdAsync(model.Id);

            if (role is null)
                throw new Exception("Role NotFound");

            var rolesPermissions = ClaimsToPermissions(await GetRoleClaimsAsync(role));

            var systemPermissions = Enum.GetValues(typeof(Permission)).Cast<Permission>();
            var modelPermissions = model.Permissions ?? Array.Empty<Permission>();


            var newPermissions = modelPermissions.Except(rolesPermissions);
            var removedPermissions = rolesPermissions.Except(modelPermissions);

            foreach (var removedPermission in removedPermissions)
            {
                var response = await RemoveClaimFromRoleAsync(role, new Claim(nameof(Permission), removedPermission.ToString()));
                results.AddRange(response);
            }

            if (results.Any())
                goto exit;

            foreach (var newpermision in newPermissions)
            {
                if (systemPermissions.Contains(newpermision))
                {
                    var response = await AddClaimToRoleAsync(role, new Claim(nameof(Permission), newpermision.ToString()));
                    results.AddRange(response);
                }
            }

            if (results.Any())
                goto exit;

            if (!role.Name.Equals(model.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                role.Name = model.Name;
                role.LastModifiedDate = Clock.Now;
                await _roleManager.UpdateAsync(role);
            }

        exit:
            return results;
        }

        public async Task<PermissionViewModel[]> GetAllPermissions()
        {

            var permissionList = new List<PermissionViewModel> { };
            var systemPermissions = await Task.FromResult(Enum.GetValues(typeof(Permission)).Cast<Permission>());


            Array.ForEach(systemPermissions.ToArray(), perm => 
            {
                permissionList.Add(new PermissionViewModel
                {
                    Category = PermissionHelper.GetPermissionCategory(perm),
                    Description = PermissionHelper.GetPermissionDescription(perm),
                    Name = Enum.GetName(typeof(Permission), perm),
                    DisplayName = Enum.GetName(typeof(Permission), perm).Replace("_", " "),
                    Id = (int)perm
                }); ;
            });


            return permissionList.ToArray();
        }
    }
}