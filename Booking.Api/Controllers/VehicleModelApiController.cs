using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    
    [Route("api/[controller]/[action]")]
    public class VehicleModelApiController : BaseController
    {
        private readonly IVehicleModelService _vehicleModelService;

        public VehicleModelApiController(IVehicleModelService vehicleModelService)
        {
            _vehicleModelService = vehicleModelService;
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleModelViewModel>>> GetAllVehicleModel([FromQuery]VehicleModelSearch model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleModelService.GetAllVehicleModelPaginatedAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<VehicleModelViewModel>> GetVehicleModel([FromQuery]VehicleModelById model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleModelService.GetVehicleModelAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<VehicleModelViewModel>>> GetVehicleModelsByMake([FromQuery]VehicleModelByMake model)
        {
            return await HandleApiOperationAsync(async () => 
            await _vehicleModelService.GetVehicleModelByMakeAsync(model).ConfigureAwait(false)).ConfigureAwait(false);
        }
        
        [HttpDelete]
        public async Task<ApiResponse<VehicleModelViewModel>> DeleteModelsById([FromQuery] DeleteVehicleModel model)
        {
            return await HandleApiOperationAsync(async () => 
            await _vehicleModelService.DeleteVehicleModelAsync(model).ConfigureAwait(false)).ConfigureAwait(false);
        }

        [HttpPut]
        public async Task<ApiResponse<EditVehicleModelViewModel>> EditVehicleModel([FromBody]EditVehicleModelViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleModelService.EditVehicleModelAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPost]
        public async Task<ApiResponse<VehicleModelViewModel>> AddVehicleModel([FromBody]VehicleModelViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _vehicleModelService.CreateVehicleModelAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}