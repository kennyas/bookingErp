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

    public class CaptainService : Service<Captain> , ICaptainService
    {
        private readonly IHttpUserService _currentUserService;

        public CaptainService(IHttpUserService currentUserService, IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }

        public async Task<List<ValidationResult>> IsVehicleAttachedToCaptain(VehicleAttachedViewModel model)
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
            var busBoy = UnitOfWork.Repository<Captain>().GetFirstOrDefault(p => p.AttachedVehicleId == vehicle.Id);

            if (busBoy != null)
            {
                results.Add(new ValidationResult("Vehicle is attached to bus boy"));
                goto ReturnToCaller;
            }
        ReturnToCaller:
            return results;

        }

        public async Task<List<ValidationResult>> AttachedToCaptain(CaptainVehicleAttachedViewModel model)
        {

            if (Guid.TryParse(model.vehilceId, out Guid vehicleId))
            {
                results.Add(new ValidationResult("invalid request"));
                goto ReturnToCaller;
            }

            if (Guid.TryParse(model.captainId, out Guid captainId))
            {

            }

            var vehicle = await UnitOfWork.Repository<Vehicle>().GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                results.Add(new ValidationResult("Vehicle has been deleted or doesn't exist"));
                goto ReturnToCaller;
            }
            var captain = await GetByIdAsync(captainId);

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

    }
}
