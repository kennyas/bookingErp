using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.Enums;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TripManagementApiController : BaseController
    {
        private readonly ITripManagementService _tripManagementService;

        public TripManagementApiController(ITripManagementService tripManagementService)
        {
            _tripManagementService = tripManagementService;
        }

        [HttpPost]
        public async Task<ApiResponse<CreateTripManagementViewModel>> Create(CreateTripManagementViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return new ApiResponse<CreateTripManagementViewModel>(errors: ListModelErrors.ToArray());

                var result = _tripManagementService.CreateTripManagement(model);

                if (!result.Any())
                {
                    //await _userEventService.PublishUserEvent(new UserCreatedIntegrationEvent(
                    //model.FirstName, model.LastName, model.Email,
                    //model.Email, model.Password, (int)model.UserType,
                    //model.EmployeeCode, model.Id,
                    //_currentUserService.GetCurrentUser().UserId));
                }
                return await Task.FromResult(new ApiResponse<CreateTripManagementViewModel>(errors: result.Select(x => x.ErrorMessage).ToArray())).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }

        [HttpGet("{Id}")]
        public async Task<ApiResponse<TripManagementViewModel>> Get([Required] string tripManagementId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!Guid.TryParse(tripManagementId, out var parsedtripInfoId))
                    return await Task.FromResult(new ApiResponse<TripManagementViewModel>(errors: "Invalid tripInformation ID")).ConfigureAwait(false);

                var result = await Task.FromResult(_tripManagementService.GetTripInformation(parsedtripInfoId)).ConfigureAwait(false);

                if (result is null)
                    return await Task.FromResult(new ApiResponse<TripManagementViewModel>(errors: "Not found", codes: ApiResponseCodes.NOT_FOUND)).ConfigureAwait(false);

                return new ApiResponse<TripManagementViewModel>(result);

            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<List<TripManagementViewModel>>> Get([FromQuery] BaseSearchViewModel searchModel)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!ModelState.IsValid)
                    return await Task.FromResult(new ApiResponse<List<TripManagementViewModel>>(errors: "Invalid request")).ConfigureAwait(false);

                var result = await Task.FromResult(_tripManagementService.GetAllTripInformation(searchModel)).ConfigureAwait(false);

                return await Task.FromResult(new ApiResponse<List<TripManagementViewModel>>(result)).ConfigureAwait(false);

            }).ConfigureAwait(false);
        }
    }
}
