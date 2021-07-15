using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class RouteHikeService : Service<RouteHike>, IRouteHikeService
    {

        private readonly IHttpUserService _currentUserService;
        public RouteHikeService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<RouteHikeCreateViewModel>> CreateRouteHikeAsync(RouteHikeCreateViewModel model)
        {
            var response = new ApiResponse<RouteHikeCreateViewModel> { Code = ApiResponseCodes.OK };
            if (model == null || model.RouteId == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");

#else
                response.Errors.Add("Invalid request");

#endif
                return response;
            }
            var existingRouteHike = FirstOrDefault(p => Guid.Equals(model?.HikeId, p?.HikeId) && Guid.Equals(p.RouteId, model.RouteId));

            if (existingRouteHike != null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Route Hike already exists");
                return response;
            }

            var RouteHike = new RouteHike
            {
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                CreatedOn = Clock.Now,
                RouteId = model.RouteId,
                HikeId = model.HikeId,
                IsActive = true,
                IsDeleted = false
            };

            var addedEntity = await AddAsync(RouteHike);
            response.ResponseCode = addedEntity > 0 ? ((int)ApiResponseCodes.OK).ToString() : ((int)ApiResponseCodes.FAILED).ToString();
            response.Description = addedEntity > 0 ? "Successful" : "No Route Hide record found";
            response.Code = ApiResponseCodes.OK;
            response.Payload = model;
            return response;
        }

        public async Task<ApiResponse<RouteHikeEditViewModel>> DeleteRouteHikeAsync(string id)
        {
            var response = new ApiResponse<RouteHikeEditViewModel> { Code = ApiResponseCodes.OK };
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

            var existingRouteHike = await GetByIdAsync(guidId);

            if (existingRouteHike == null)
            {
                //response.ResponseCode = ((int)ApiResponseCodes.NOT_FOUND).ToString();
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Route Hide does not exists or has been deleted");
                return response;
            }

            existingRouteHike.IsDeleted = true;
            existingRouteHike.ModifiedOn = Clock.Now;
            existingRouteHike.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingRouteHike.DeletedBy = _currentUserService.GetCurrentUser().UserId;
            existingRouteHike.DeletedOn = Clock.Now;
            this.Delete(existingRouteHike);


            response.Description = "Successful";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = ((int)ApiResponseCodes.OK).ToString();
            response.Payload = null;
            return response;
        }

        public async Task<ApiResponse<RouteHikeEditViewModel>> EditRouteHikeAsync(RouteHikeEditViewModel model)
        {
            var response = new ApiResponse<RouteHikeEditViewModel> { Code = ApiResponseCodes.OK };
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
            if (!Guid.TryParse(model.Id, out Guid guidId) || string.IsNullOrWhiteSpace(model.Id))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
#if DEBUG
                response.Errors.Add("model cannot be null or empty");
#else
                response.Errors.Add("invalid request");
#endif
                return response;
            }
            var existingRouteHike = await GetByIdAsync(guidId);

            if (existingRouteHike == null)
            {
                //response.ResponseCode = ((int)ApiResponseCodes.NOT_FOUND).ToString();
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Route Hike does not exists or has been deleted");
                return response;
            }

            existingRouteHike.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingRouteHike.ModifiedOn = Clock.Now;
            existingRouteHike.RouteId = model.RouteId;
            existingRouteHike.HikeId = model.HikeId;

            var addedEntity = await UpdateAsync(existingRouteHike);

            response.Description = addedEntity > 0 ? "Successful" : "No Route Hike record found";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = addedEntity > 0 ? ((int)ApiResponseCodes.OK).ToString() : ((int)ApiResponseCodes.NOT_FOUND).ToString();
            response.Payload = model;
            return response;
        }

        public async Task<ApiResponse<RouteHikeCreateViewModel>> GetRouteHikeAsync(string id)
        {
            var response = new ApiResponse<RouteHikeCreateViewModel> { Code = ApiResponseCodes.OK };
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

            var existingRouteHike = await GetByIdAsync(guidId);


            if (existingRouteHike == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add("Route Hike does not exists");
                //response.ResponseCode = ((int)ApiResponseCodes.NOT_FOUND).ToString();
                return response;
            }

            var modelState = (RouteHikeCreateViewModel)existingRouteHike;
            response.Description = "Successful";
            response.Code = ApiResponseCodes.OK;
            response.ResponseCode = ((int)ApiResponseCodes.OK).ToString();
            response.Payload = modelState;
            return response;
        }
    }
}
