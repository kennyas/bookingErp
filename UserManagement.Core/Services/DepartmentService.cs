using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;
using System.Linq;
using UserManagement.Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Tornado.Shared.AspNetCore;

namespace UserManagement.Core.Services
{
    public class DepartmentService : Service<Department>, IDepartmentService
    {
        private readonly IHttpUserService _currentUserService;

        public DepartmentService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }
        public async Task<ApiResponse<DepartmentViewModel>> CreateDepartmentAsync(DepartmentViewModel model)
        {
            var response = new ApiResponse<DepartmentViewModel> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");
#else
                response.Errors.Add("invalid request");
#endif
                return response;
            }

            var existingDepartment = FirstOrDefault(p => string.Equals(model.Name, p?.Name, StringComparison.OrdinalIgnoreCase));

            if (existingDepartment != null)
            {
                response.Errors.Add("Department already exists");
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Department already exists";
                return response;
            }

            var department = new Department
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                Name = model.Name,
                Description = model.Description
            };
            var addedEntity = await AddAsync(department);

            model.Id = department.Id.ToString();
            response.Description = addedEntity > 0 ? "Successful" : "No Department record found";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = addedEntity > 0 ? ((int)ApiResponseCodes.OK).ToString() : ((int)ApiResponseCodes.NOT_FOUND).ToString();
            response.Payload = model;
            return response;
        }

        public async Task<ApiResponse<DepartmentViewModel>> EditDepartmentAsync(DepartmentViewModel model)
        {
            var response = new ApiResponse<DepartmentViewModel> { Code = ApiResponseCodes.OK };
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");
#else
                response.Errors.Add("invalid request");
#endif
                return response;
            }
            //just get the departmwnt by Id



            if (!Guid.TryParse(model.Id, out Guid guidId) || string.IsNullOrEmpty(model.Id))
            {
                return new
                         ApiResponse<DepartmentViewModel>(codes: ApiResponseCodes.EXCEPTION, errors: "Unrecognised Guid format", message: "Guid format not recognised");

            }
            var existingDepartment = GetById(guidId);

            if (existingDepartment == null)
            {
                //response.ResponseCode = ((int)ApiResponseCodes.INVALID_REQUEST).ToString();
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Department does not exists");
                response.Description = "Department does not exists";
                return response;
            }

            existingDepartment.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingDepartment.ModifiedOn = Clock.Now;
            existingDepartment.Name = model.Name;
            existingDepartment.Description = model.Description;

            var addedEntity = await UpdateAsync(existingDepartment);

            response.Description = addedEntity > 0 ? "Successful" : "No Department record found";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = addedEntity > 0 ? ((int)ApiResponseCodes.OK).ToString() : ((int)ApiResponseCodes.NOT_FOUND).ToString();
            response.Payload = model;
            return response;
        }

        public async Task<ApiResponse<DepartmentViewModel>> DeleteDepartmentAsync(string id)
        {
            var response = new ApiResponse<DepartmentViewModel> { Code = ApiResponseCodes.OK };
            if (!Guid.TryParse(id, out Guid guidId) || string.IsNullOrEmpty(id))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");
#else
                response.Errors.Add("invalid request");
#endif
                return response;
            }

            var existingDepartment = await this.GetByIdAsync(guidId);

            if (existingDepartment == null)
            {
                //response.ResponseCode = ((int)ApiResponseCodes.INVALID_REQUEST).ToString();
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Department does not exists");
                response.Description = "Department does not exists";
                return response;
            }

            //modify properties to update 
            existingDepartment.IsDeleted = true;
            existingDepartment.ModifiedOn = Clock.Now;

            //modified by current user
            //existingDepartment.ModifiedBy = 
            Delete(existingDepartment);

            response.Description = "Successful";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = ((int)ApiResponseCodes.OK).ToString();
            response.Payload = null;
            return response;
        }

        /// <summary>
        /// Get should take an id of the Department..
        /// if we have to use search that takes more than one parameters..
        /// then the can use the GetAllDepartmentAsync method
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ApiResponse<DepartmentViewModel>> GetDepartmentAsync(string id)
        {
            var response = new ApiResponse<DepartmentViewModel> { Code = ApiResponseCodes.OK };


            if (!Guid.TryParse(id, out Guid guidId) || string.IsNullOrEmpty(id))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");
#else
                response.Errors.Add("invalid request");
#endif
                return response;
            }

            var existingdepartment = (DepartmentViewModel)await GetByIdAsync(guidId);

            if (existingdepartment == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Department does not exists");
                response.Description = "Department does not exists";
                return response;
            }

            response.Description = "Successful";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = ((int)ApiResponseCodes.OK).ToString();
            response.Payload = existingdepartment;
            return response;
        }
        /// <summary>
        /// Search View Model takes in a search view which inherits from BaseSearchViewModel
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<ApiResponse<PaginatedList<DepartmentViewModel>>> GetAllDepartmentAsync(BaseSearchViewModel searchModel)
        {

            var response = new ApiResponse<PaginatedList<DepartmentViewModel>> { Code = ApiResponseCodes.OK };

            var keyword = string.IsNullOrEmpty(searchModel.Keyword) ? null : searchModel.Keyword.ToLower();
            int pageStart = searchModel.PageIndex ?? 1;
            int pageEnd = searchModel.PageTotal ?? 50;


            var modelEntities = SqlQuery<DepartmentDto>("[dbo].[Sp_GetDepartments]  @p0, @p1, @p2",
                                                     keyword, pageStart, pageEnd)
                                                    .Select(p => (DepartmentViewModel)p)
                                                    .ToList();

            response.Description = modelEntities != null && !modelEntities.Any() ? "Successful" : "No department record found";
            response.Code = ApiResponseCodes.OK;
            response.TotalCount = modelEntities != null && !modelEntities.Any() ? 0 : modelEntities.FirstOrDefault().TotalCount;

            response.Payload = modelEntities.ToPaginatedList(pageStart, pageEnd, response.TotalCount);

            return await Task.FromResult(response);          
        }
    }
}
