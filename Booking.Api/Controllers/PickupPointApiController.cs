using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.AspNetCore.Policy;
using Tornado.Shared.EF;
using Tornado.Shared.Helpers;
using Tornado.Shared.ViewModels;

namespace Booking.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PickupPointApiController : BaseController
    {
        private readonly IPickupPointService _pickupPointService;

        public PickupPointApiController(IPickupPointService pickupPointService)
        {
            _pickupPointService = pickupPointService;
        }

        [HttpPost]
        //[RequiresPermission(Permission.CREATE_PICKUPPOINT)]
        public async Task<ApiResponse<PickupPointViewModel>> AddPickupPoint(PickupPointViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.CreatePickupPointAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpPut]
        [RequiresPermission(Permission.EDIT_PICKUPPOINT)]
        public async Task<ApiResponse<PickupPointViewModel>> EditPickupPoint(PickupPointViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.EditPickupPointAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpDelete]
        [RequiresPermission(Permission.DELETE_PICKUPPOINT)]
        public async Task<ApiResponse<PickupPointViewModel>> DeletePickupPoint([FromQuery] DeletePickupPointViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.DeletePickupPointAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPoint([FromQuery]GetPickupPointByIdViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetPickupPointAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetAllPickupPoint([FromQuery]PickupPointPaginatedViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetAllPickupPointsAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointsByState([FromQuery]SearchPickupPointViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetPickupPointsByStateAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
        [HttpGet]
        public async Task<ApiResponse<PaginatedList<PickupPointViewModel>>> GetPickupPointsByTitle([FromQuery]SearchPickupPointViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetPickupPointsByTitleAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetPickupPointsByDeparturePoint([FromQuery]GetRoutePickUpPointByDepartureId model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetDesitinationPickupPointsByDepartureAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetPickupPointsByDestinationPoint([FromQuery]GetDeparturePickUpPointByDestinationId model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetDeparturePickupPointsByDestinationAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetDeparturePickupPoints([FromQuery]GetDeparturePickUpPoints model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetDeparturePickupPointsAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }

        [HttpGet]
        public async Task<ApiResponse<PaginatedList<ListPickupPointViewModel>>> GetOrderedDeparturePickupPoints([FromQuery]GetOrderedDeparturePickUpPoints model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                return await _pickupPointService.GetOrderedDeparturePickupPointsAsync(model).ConfigureAwait(false);
            }).ConfigureAwait(false);
        }
    }
}
