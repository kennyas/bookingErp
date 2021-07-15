using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class VehicleExcludedSeatService : Service<VehicleExcludedSeat>, IVehicleExcludedSeatService
    {
        private readonly IHttpUserService _currentUserService;
        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public int[] GetExcludedSeats(Guid vehicleModelId)
        {
            return GetAll()
             .Where(x => x.VehicleModelId == vehicleModelId && x.IsActive)
             .Select(x => x.SeatNumber).ToArray();
        }

        public SeatDetail[] GetExcludedSeatDetails(Guid vehicleModelId)
        {
            return GetAll()
             .Where(x => x.VehicleModelId == vehicleModelId)
             .Select(x => new SeatDetail { SeatNumber = x.SeatNumber, IsActive = x.IsActive }).ToArray();
        }
        public VehicleExcludedSeatService(IUnitOfWork unitOfWork, IHttpUserService currentUserService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
        }


        public async Task<ApiResponse<VehicleExcludedSeatsViewModel>> GetVehicleExcludedSeat(string id)
        {
            var response = new ApiResponse<VehicleExcludedSeatsViewModel>(codes: ApiResponseCodes.OK);

            if (!Guid.TryParse(id, out Guid vehicleModelId))
            {
                return new ApiResponse<VehicleExcludedSeatsViewModel>(codes: ApiResponseCodes.INVALID_REQUEST, message: "Invalid request");
            }

            var excludedSeatsVM = new VehicleExcludedSeatsViewModel();

            var seatDetails = new List<SeatDetail>();

            await semaphoreSlim.WaitAsync();
            try
            {
                var vehicleModel = await UnitOfWork.Repository<VehicleModel>().GetByIdAsync(vehicleModelId);


                for (int seat = 1; seat <= vehicleModel.NoOfSeats ; seat++)
                {
                    //var seatNumber = ++seat;

                    var excludedSeat = UnitOfWork.Repository<VehicleExcludedSeat>().GetFirstOrDefault(p => p.SeatNumber == seat && p.VehicleModelId == vehicleModelId);

                    if (excludedSeat == null)
                    {
                        try
                        {
                            var newEntity = await AddAsync(new VehicleExcludedSeat { IsActive = false, CreatedOn = Clock.Now, CreatedBy = _currentUserService.GetCurrentUser().UserId, SeatNumber = seat, VehicleModelId = vehicleModelId });

                            seatDetails.Add(new SeatDetail { IsActive = false, SeatNumber = seat });
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                        continue;
                    }

                    seatDetails.Add(new SeatDetail { IsActive = excludedSeat.IsActive, SeatNumber = excludedSeat.SeatNumber });
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }


            excludedSeatsVM.SeatDetail = seatDetails;

            response.Payload = excludedSeatsVM;
            response.Code = ApiResponseCodes.OK;

            return response;
        }
        public async Task<List<ValidationResult>> UpdateVehicleExcludedSeats(AddVehicleExcludedSeatsRequestViewModel model)
        {
            var response = new ApiResponse<VehicleExcludedSeatsViewModel>(codes: ApiResponseCodes.NOT_FOUND);

            var currentActiveSeats = GetExcludedSeatDetails(model.VehicleModelId).Where(p => p.IsActive);

            var allRemovedSeats = GetExcludedSeatDetails(model.VehicleModelId).Except(model.SeatDetails);


            var removedSeats = model.SeatDetails.Except(currentActiveSeats);
            var excludedSeats = currentActiveSeats.Except(model.SeatDetails);

            foreach (var excludedSeat in excludedSeats)
            {
                var entity = FirstOrDefault(p => p.VehicleModelId == model.VehicleModelId && p.SeatNumber == excludedSeat.SeatNumber);
                if (!excludedSeat.IsActive) continue;
                entity.IsActive = true;
                entity.ModifiedOn = Clock.Now;
                entity.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

                var message = await UpdateAsync(entity) > 0 ? "Successful!" : $"Could not update seat-{excludedSeat.SeatNumber}";

                results.Add(new ValidationResult(message));
            }


            foreach (var removedSeat in removedSeats)
            {
                var entity = FirstOrDefault(p => p.VehicleModelId == model.VehicleModelId && p.SeatNumber == removedSeat.SeatNumber);
                if (removedSeat.IsActive) continue;

                entity.IsActive = false;
                entity.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
                entity.ModifiedOn = Clock.Now;


                var message = await UpdateAsync(entity) > 0 ? "Successful!" : $"Could not update seat-{removedSeat.SeatNumber}";

                results.Add(new ValidationResult(message));
            }

            return results;
        }
    }
}