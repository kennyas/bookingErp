using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Timing;

namespace Booking.Core.Services
{
    public class BusBoyService : Service<BusBoy>, IBusBoyService
    {
        private readonly IHttpUserService _currentUserService;

        public BusBoyService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }
        public async Task<List<ValidationResult>> AttachVehicleToBusBoy(BusBoyVehicleAttachedViewModel model)
        {

            if (Guid.TryParse(model.vehilceId, out Guid vehicleId)
                )
            {
                results.Add(new ValidationResult("invalid request"));
                goto ReturnToCaller;
            }

            if (Guid.TryParse(model.busboyId, out Guid busboyId))
            {

            }

            var vehicle = await UnitOfWork.Repository<Vehicle>().GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                results.Add(new ValidationResult("Vehicle has been deleted or doesn't exist"));
                goto ReturnToCaller;
            }
            var captain = await GetByIdAsync(busboyId);

            if (captain == null)
            {
                results.Add(new ValidationResult("Captain does not exist"));
                goto ReturnToCaller;
            }

            captain.AttachedVehicleId = vehicle.Id;
            captain.ModifiedOn = Clock.Now;
            captain.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            await UpdateAsync(captain);

        ReturnToCaller:
            return results;

        }

        public async Task<List<ValidationResult>> IsVehicleAttachedToBusboy(VehicleAttachedViewModel model)
        {

            if (Guid.TryParse(model.vehilceId, out Guid vehicleId))
            {
                results.Add(new ValidationResult("invalid request"));
                goto ReturnToCaller;
            }

            var vehicle = await GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                results.Add(new ValidationResult("Vehicle has been deleted or doesn't exist"));
                goto ReturnToCaller;
            }
            var busBoy = UnitOfWork.Repository<BusBoy>().GetFirstOrDefault
                (p => p.AttachedVehicleId == vehicle.Id);

            if (busBoy != null)
            {
                results.Add(new ValidationResult("Vehicle is attached to bus boy"));
                goto ReturnToCaller;
            }
            ReturnToCaller:
            return results;
        }
    }
}