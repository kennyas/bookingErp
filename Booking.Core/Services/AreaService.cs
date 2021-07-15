using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.ViewModels;
using System.Linq;
using Tornado.Shared.Enums;
using System.Threading.Tasks;
using Tornado.Shared.Timing;
using Tornado.Shared.AspNetCore;
using Booking.Core.Validators;
using Booking.Core.Dtos;

namespace Booking.Core.Services
{
    public class AreaService : Service<Area>, IAreaService
    {
        private readonly IHttpUserService _currentUserService;

        public AreaService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }


        public async Task<ApiResponse<PaginatedList<AreaListViewModel>>> GetAreas(AreaSearchViewModel model)
        {
            var response = new ApiResponse<PaginatedList<AreaListViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                //response.Errors.Add("Id can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex = model.PageIndex <= 0 ? 1 : model.PageIndex;
            model.PageTotal ??= 10;

            var areas = await Task.FromResult(from area in UnitOfWork.Repository<Area>().GetAll()
                                              join state in UnitOfWork.Repository<State>().GetAll()

                                              on area.StateId equals state.Id

                                              where string.IsNullOrEmpty(model.Keyword) || (state.Name.Contains(model.Keyword, StringComparison.OrdinalIgnoreCase)
                                                              || (area.Title.Contains(model.Keyword, StringComparison.OrdinalIgnoreCase)))


                                              select new AreaListViewModel
                                              {
                                                  Title = area.Title,
                                                  StateId = state.Id,
                                                  StateName = state.Name,
                                                  Id = area.Id,
                                                  Code = area.AreaCode,
                                                  StateCode = state.Code
                                              }).ConfigureAwait(false);


            response.Payload = areas.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                 pageIndex: response.Payload.PageIndex,
                 pageSize: response.Payload.PageSize,
                 totalPageCount: response.Payload.TotalPageCount,   
                 totalCount: response.Payload.TotalCount);

            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No pickup point was found";

            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<CreateAreaViewModel>> CreateArea(CreateAreaViewModel model)
        {
            var response = new ApiResponse<CreateAreaViewModel>(codes: ApiResponseCodes.OK);
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid request";
                goto ReturnToCaller;
            }

            var validationResult = new CreateAreaValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.StateId, out Guid stateGuid))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid request";

                goto ReturnToCaller;
            }

            var existingArea = FirstOrDefault(area => area.Title.Equals(model.Title, StringComparison.OrdinalIgnoreCase));

            if (existingArea != null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Description = "Area already exists";
                goto ReturnToCaller;
            }

            var area = new Area
            {
                Title = model.Title,
                Description = model.Description,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                AreaCode = model.AreaCode,
                CreatedOn = Clock.Now,
                StateId = stateGuid
            };
            var addedEntity = await AddAsync(area);

            model.Id = addedEntity > 0 ? area.Id : Guid.Empty;
            return new ApiResponse<CreateAreaViewModel>(codes: addedEntity > 0 ? ApiResponseCodes.OK : ApiResponseCodes.FAILED, 
                                                         message: addedEntity > 0 ? "Successful" : "Failed", data: model);

            
            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;

        }

        public async Task<ApiResponse<EditAreaViewModel>> EditArea(EditAreaViewModel model)
        {
            var response = new ApiResponse<EditAreaViewModel>(codes: ApiResponseCodes.OK);
            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid request";
                goto ReturnToCaller;
            }
            if (!Guid.TryParse(model.StateId, out Guid stateGuid) || !Guid.TryParse(model.Id, out Guid id))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid request";
                goto ReturnToCaller;
            }

            var validationResult = new EditAreaValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }


            var existingArea = FirstOrDefault(area => string.Equals(area.Title, model.Title, StringComparison.OrdinalIgnoreCase) && (!Equals(area.Id, id)));

            if (existingArea != null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Description = "Area with specified Title already exists";
                goto ReturnToCaller;
            }

            var entity = GetById(id);
            if (entity == null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Description = "Area does not exist";
                goto ReturnToCaller;
            }
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.StateId = stateGuid;
            entity.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            entity.ModifiedOn = Clock.Now;

            var updatedEntity = await UpdateAsync(entity);


            return new ApiResponse<EditAreaViewModel>(codes: updatedEntity > 0 ? ApiResponseCodes.OK : ApiResponseCodes.FAILED, message: updatedEntity > 0 ? "Successful" : "Failed", data: model);

            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<AreaViewModel>> GetArea(string id)
        {
            var response = new ApiResponse<AreaViewModel>(codes: ApiResponseCodes.OK);
            if ( !Guid.TryParse(id, out Guid guid))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid request";
                goto ReturnToCaller;
            }

            var entity = await Task.FromResult(from area in UnitOfWork.Repository<Area>().GetAll()
                                         join state in UnitOfWork.Repository<State>().GetAll()
                                         on area.StateId equals state.Id
                                         where Equals(guid, area.Id) && area.IsDeleted == false

                                         select new AreaViewModel
                                         {
                                             AreaCode = area.AreaCode,
                                             Description = area.Description,
                                             StateName = state.Name,
                                             StateId = state.Id.ToString(),
                                             Id = area.Id.ToString(),
                                             Title = area.Title
                                         });

            if (entity.FirstOrDefault() == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Description = "Area does not exist";
                goto ReturnToCaller;
            }

            return new ApiResponse<AreaViewModel>(message: "Successful", codes: ApiResponseCodes.OK, data: entity.FirstOrDefault());

            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }


        public async Task<ApiResponse<AreaViewModel>> DeleteArea(string id)
        {
            var response = new ApiResponse<AreaViewModel>(codes: ApiResponseCodes.OK);
            if (!Guid.TryParse(id, out Guid guid))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid request";
                goto ReturnToCaller;
            }
            var entity = GetById(guid);
            if (entity == null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Description = "Area does not exist";
                goto ReturnToCaller;
            }

            entity.IsDeleted = true;
            entity.DeletedOn = Clock.Now;
            entity.DeletedBy = _currentUserService.GetCurrentUser().UserId;


            var updatedEntity = await UpdateAsync(entity);

            return new ApiResponse<AreaViewModel>(codes: updatedEntity > 0 ? ApiResponseCodes.OK : ApiResponseCodes.FAILED, message: updatedEntity > 0 ? "Successful" : "Failed");


            ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }
    }
}
