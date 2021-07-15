using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]

    public class VehicleMakeApiController : BaseController
    {
        private readonly IVehicleMakeService _vehicleMakeService;
        public VehicleMakeApiController(IVehicleMakeService vehicleMakeService)
        {
            _vehicleMakeService = vehicleMakeService;
        }
        [HttpPost]
        public async Task<ApiResponse<VehicleMakeViewModel>> AddVehicleMake([FromBody]VehicleMakeViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {

                return await _vehicleMakeService.CreateVehicleMakeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleMakeViewModel>>> SearchVehicleMake([FromQuery]VehicleMakeSearchViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleMakeService.SearchVehicleMakeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleMakeViewModel>>> GetAllVehicleMake([FromQuery]BasePaginatedViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleMakeService.GetAllVehicleMakeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        
        [HttpGet]
        public async Task<ApiResponse<VehicleMakeViewModel>> GetVehicleMake([FromQuery]GetVehicleMakeByIdViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleMakeService.GetVehicleMakeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ApiResponse<EditVehicleMakeViewModel>> EditVehicleMake([FromBody]EditVehicleMakeViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleMakeService.EditVehicleMakeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        public async Task<ApiResponse<VehicleMakeViewModel>> DeleteVehicleMake([FromQuery]DeleteVehicleMakeViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleMakeService.DeleteVehicleMakeAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}