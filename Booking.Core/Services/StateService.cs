using Booking.Core.Dtos;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class StateService : Service<State>, IStateService
    {
        private readonly IHttpUserService _currentUserService;

        public StateService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<StateViewModel>> CreateState(StateCreateModel model)
        {
            var response = new ApiResponse<StateViewModel> { Code = ApiResponseCodes.OK };
            if (model == null || string.IsNullOrWhiteSpace(model?.CountryId))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add(model?.CountryId == null ? "Indicating country does not exist" : "model cannot be null or empty");

#else
                response.Errors.Add("Invalid request or indicating country does not exist");

#endif
                return response;
            }
            //state name in the same country
            Guid.TryParse(model.CountryId, out Guid guidId);

            var existingState = FirstOrDefault(p => string.Equals(model?.Name, p?.Name, StringComparison.OrdinalIgnoreCase) &&
                                Guid.Equals(p.CountryId, guidId));

            if (existingState != null)
            {
                //response.Code = ApiResponseCodes.INVALID_REQUEST;
                //response.Errors.Add("State already exists");
                //return response;
                return new ApiResponse<StateViewModel>(errors: "State already exists", codes: ApiResponseCodes.EXCEPTION);
            }

            var countryUnavailable = await UnitOfWork.Repository<Country>().GetByIdAsync(guidId) is null;

            if (countryUnavailable) {
                //response.Code = ApiResponseCodes.INVALID_REQUEST;
                //response.Errors.Add("Country associated not found or previously deleted");
                //return response;
                return new ApiResponse<StateViewModel>(errors: "Country associated not found or previously deleted", codes: ApiResponseCodes.EXCEPTION);
            }

            var state = new State
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                Code = model.Code,
                Name = model.Name,
                Description = model.Description,
                CountryId = guidId,
                CreatedOn = Clock.Now                
            };

            var addedEntity = await AddAsync(state);
            var stateviewmodel = new StateViewModel
            {
                CreatedBy = state.CreatedBy,
                CountryId = state.CountryId,
                Code = state.Code,
                Description = state.Description,
                Name = state.Name                
            };
            stateviewmodel.Id = state.Id.ToString();

            return addedEntity > 0 ? new ApiResponse<StateViewModel>(message: "Successful",
                           codes: ApiResponseCodes.OK, data: stateviewmodel)
               : new ApiResponse<StateViewModel>(errors: "Could not add state", codes: ApiResponseCodes.FAILED);


            //response.ResponseCode = addedEntity > 0 ? ((int)ApiResponseCodes.OK).ToString() : ((int)ApiResponseCodes.FAILED).ToString();
            //response.Description = addedEntity > 0 ? "Successful" : "No state record found";
            //response.Code = ApiResponseCodes.OK;
            //response.Payload = model;           
            //return response;
        }

        public async Task<ApiResponse<StateViewModel>> EditStateAsync(StateEditModel model)
        {
            var response = new ApiResponse<StateViewModel> { Code = ApiResponseCodes.OK };
            if (model == null || model.Id == null)
            {
                return new
                    ApiResponse<StateViewModel>(codes: ApiResponseCodes.EXCEPTION, errors: "invalid request", message: "Guid format not recognised");
            }

            if (!Guid.TryParse(model.Id, out Guid guidId) || string.IsNullOrEmpty(model.Id))
            {
                return new 
                    ApiResponse<StateViewModel>(codes: ApiResponseCodes.EXCEPTION, errors: "Unrecognised Guid format", message: "Guid format not recognised");
            }


            var existingstate = await GetByIdAsync(guidId);

            if (existingstate == null)
            {
                return new
                 ApiResponse<StateViewModel>(codes: ApiResponseCodes.NOT_FOUND, errors: "Could not find state", message: "Could not find state");
            }

            //e.g I should be able to create another Delta... If its a state in Ghana
            var conflictingState = FirstOrDefault(p => string.Equals(p.Name, model.Name, StringComparison.OrdinalIgnoreCase)
                                                    && !Equals(p.Id, model.Id) && Equals(p.CountryId, model.CountryId));

            if (conflictingState != null)
            {
                //response.ResponseCode = ((int)ApiResponseCodes.INVALID_REQUEST).ToString();
                // response.Code = ApiResponseCodes.EXCEPTION;
                // response.Errors.Add("State already exists in the country");
                // return response;
                return new
                  ApiResponse<StateViewModel>(codes: ApiResponseCodes.EXCEPTION, errors: "State already exists in the country", message: "State already exists in the country");
            }

            existingstate.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingstate.Name = model.Name;
            existingstate.CountryId = model.CountryId;
            existingstate.Description = model.Description;

            var existingEntity = await UpdateAsync(existingstate);

            model.Id =  existingstate.Id.ToString();

            var stateviewmodel = new StateViewModel
            {
                CreatedBy = existingstate.CreatedBy,
                CountryId = existingstate.CountryId,
                Code = existingstate.Code,
                Description = existingstate.Description,
                Name = existingstate.Name,
                ModifiedBy = existingstate.ModifiedBy                
            };

            response.Description = existingEntity > 0 ? "Successful" : "No state record found";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = existingEntity > 0 ? ((int)ApiResponseCodes.OK).ToString() : ((int)ApiResponseCodes.NOT_FOUND).ToString();
            response.Payload = stateviewmodel;
            return response;
        }

        public async Task<ApiResponse<StateViewModel>> DeleteStateAsync(string id)
        {
            var response = new ApiResponse<StateViewModel> { Code = ApiResponseCodes.OK };
            if (!Guid.TryParse(id, out Guid guidId) || string.IsNullOrWhiteSpace(id))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");
#else
                response.Errors.Add("invalid request");
#endif
                return response;
            }

            var existingstate = await GetByIdAsync(guidId);

            if (existingstate == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("State does not exists or has been deleted");
                return response;
            }

            existingstate.IsDeleted = true;
            existingstate.ModifiedOn = Clock.Now;
            existingstate.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            this.Delete(existingstate);


            response.Description = "Successful";
            response.Code = ApiResponseCodes.OK;
            response.Payload = null;
            return response;
        }

        public async Task<ApiResponse<StateViewModel>> GetStateAsync(string id)
        {
            var response = new ApiResponse<StateViewModel> { Code = ApiResponseCodes.OK };
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

            var existingstate = await GetByIdAsync(guidId);


            if (existingstate == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("State does not exists");
                return response;
            }

            var modelState = (StateViewModel)existingstate;
            response.Description = "Successful";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = ((int)ApiResponseCodes.OK).ToString();
            response.Payload = modelState;
            return response;
        }

        public async Task<ApiResponse<PaginatedList<StateViewModel>>> GetAllStateAsync(BaseSearchViewModel searchModel)
        {
            //var response = new ApiResponse<PaginatedList<StateViewModel>> { Code = ApiResponseCodes.OK };

            var keyword = string.IsNullOrEmpty(searchModel.Keyword) ? null : searchModel.Keyword.ToLower();
            int pageStart = searchModel.PageIndex == null ? 0 : searchModel.PageIndex.Value - 1;
            int pageEnd = searchModel.PageTotal ?? 50;


            var modelEntities = SqlQuery<StateDto>("[dbo].[Sp_GetStates]  @p0, @p1, @p2",
                                                     keyword, pageStart, pageEnd)
                                                    .Select(p => (StateViewModel)p)
                                                    .ToList();

            //response.Description = modelEntities != null && modelEntities.Any() ? "Successful" : "No state record found";
            //response.Code = ApiResponseCodes.OK;
            //response.TotalCount = modelEntities != null && modelEntities.Any() ? modelEntities.FirstOrDefault().TotalCount : 0;

            //response.Payload = modelEntities.ToPaginatedList(pageStart, pageEnd, response.TotalCount);


            var count = modelEntities.Count();
            return await Task.FromResult(new ApiResponse<PaginatedList<StateViewModel>>(message: modelEntities != null && modelEntities.Any() ? "Successful" : "No state record found",
                                                                                codes: ApiResponseCodes.OK,
                                                                                totalCount: modelEntities != null && !modelEntities.Any() ? 0 : modelEntities.FirstOrDefault().TotalCount,
                                                                                data: modelEntities.ToPaginatedList(pageStart, pageEnd, count)));

        }
    }
}

