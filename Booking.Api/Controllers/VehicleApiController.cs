using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]

    public class VehicleApiController : BaseController
    {
        private readonly IVehicleService _vehicleService;

        public VehicleApiController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPost]
        public async Task<ApiResponse<VehicleViewModel>> AddVehicle([FromBody]VehicleViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.CreateVehicleAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ApiResponse<VehicleViewModel>> EditVehicle([FromBody]EditVehicleViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.EditVehicleAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ApiResponse<VehicleViewModel>> DeleteVehicle([FromQuery]DeleteVehicleViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.DeleteVehicleAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleViewModel>>> GetAllVehicle([FromQuery]BasePaginatedViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.GetAllVehicleAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }


        [HttpGet]
        public async Task<ApiResponse<VehicleViewModel>> GetVehicleByChassisNumber([FromQuery]SearchVehicleByChassisNumberViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.GetVehicleByChasisNumberAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleViewModel>>> GetVehicleByPartnerId([FromQuery]GetVehicleByPartnerIdViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.GetVehicleByPartnerIdAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<VehicleViewModel>> GetVehicleByRegistrationNumber([FromQuery]SearchVehicleByRegistrationNumberViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.GetVehicleByRegistrationNumberAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleViewModel>>> SearchVehicle([FromQuery]VehiclePaginatedViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.SearchVehicleAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<List<TripVehicleViewModel>>> GetAllUnattachedVehicles(string vehicleModelId)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleService.GetUnattachedVehicles(new TripVehicleSearchModel { VehicleModelId = vehicleModelId}).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

    }
}
