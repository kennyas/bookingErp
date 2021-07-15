using Booking.Core.Enums;
using Booking.Core.Helpers;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.ViewModels;

namespace Booking.Core.Services
{
    public class TripManagementService : Service<TripManagement>, ITripManagementService
    {
        private readonly ITripService _tripService;
        private readonly IHttpUserService _currentUserService;

        private static readonly object _locker = new object();

        public TripManagementService(IUnitOfWork unitOfWork,
        ITripService tripService,
        IHttpUserService currentUserService) : base(unitOfWork)
        {
            _tripService = tripService;
            _currentUserService = currentUserService;
        }

        public List<ValidationResult> CreateTripManagement(CreateTripManagementViewModel model)
        {
            if (!IsValid(model))
                return results;

            lock (_locker)
            {
                var trip = _tripService.GetById(model.TripId);

                if (trip is null)
                {
                    results.Add(new ValidationResult("Trip not found"));
                    return results;
                }

                if (UnitOfWork.Repository<TripManagement>()
                                    .Query(p => p.TripId == model.TripId
                                    && p.DepartureDate == model.DepartureDate).Any())
                {
                    results.Add(new ValidationResult("Maximum trips exceeded, Please try again at a later time."));
                    return results;
                }

                if (model.DepartureDate <= Clock.Now)
                {
                    results.Add(new ValidationResult("Departure date must be in the future."));
                    return results;
                }

                var tripManagement = new TripManagement
                {
                    DepartureDate = model.DepartureDate,
                    TripId = model.TripId,
                    CreatedOn = Clock.Now,
                    CreatedBy = _currentUserService.GetCurrentUser().UserId,
                    Status = TripManagementStatus.PENDING,
                };

                if (Guid.TryParse(model.DriverId, out Guid driverId) && driverId != Guid.Empty)
                {
                    var driver = UnitOfWork.Repository<Captain>().GetFirstOrDefault(x => x.Id == driverId);

                    if (driver != null)
                        tripManagement.DriverId = driver.Id;
                }

                if (Guid.TryParse(model.BusBoyId, out Guid busBoyId) && busBoyId != Guid.Empty)
                {
                    var busBoy = UnitOfWork.Repository<BusBoy>().GetFirstOrDefault(x => x.Id == busBoyId);

                    if (busBoy != null)
                        tripManagement.BusBoyId = busBoy.Id;
                }

                Add(tripManagement);
                model.Id = tripManagement.Id;
            }

            return results;
        }

        public TripManagementViewModel GetTripInformation(Guid tripInfoId)
        {
            var tripInfo = UnitOfWork.Repository<TripManagement>()
                .GetSingleIncluding(x => x.Id == tripInfoId, x => x.Driver, x => x.BusBoy,
                x => x.Trip);

            TripManagementViewModel result = null;

            if (tripInfo is null)
                return result;

            result = new TripManagementViewModel
            {
                Id = tripInfo.Id,
                TripId = tripInfo.TripId,
                Status = tripInfo.Status.GetDescription(),
                DepartureDate = tripInfo.DepartureDate.ToString(),
                BusBoy = $"{tripInfo?.BusBoy?.FirstName} {tripInfo?.BusBoy?.LastName}",
                Driver = $"{tripInfo?.Driver?.FirstName} {tripInfo.BusBoy?.LastName}",
            };

            return result;
        }

        public List<TripManagementViewModel> GetAllTripInformation(BaseSearchViewModel searchModel)
        {
            int pageStart = searchModel.PageIndex <= 0 ? 1 : searchModel.PageIndex.Value;
            int pageEnd = searchModel.PageTotal ?? 50;

            var isStatus = Enum.TryParse(searchModel?.Keyword?.ToUpper() ?? "", out TripManagementStatus status);

            var request = from tripInfo in GetAll()

                          join Trip trip in UnitOfWork.Repository<Trip>().GetAll()
                          on tripInfo.TripId equals trip.Id

                          join dr in UnitOfWork.Repository<Captain>().GetAll()
                          on tripInfo.DriverId equals dr.Id into drGrp
                          from driver in drGrp.DefaultIfEmpty()

                          join bb in UnitOfWork.Repository<BusBoy>().GetAll()
                         on tripInfo.BusBoyId equals bb.Id into bbGrp
                          from busBoy in bbGrp.DefaultIfEmpty()

                          where (string.IsNullOrWhiteSpace(searchModel.Keyword)
                          || trip.Title.Contains(searchModel.Keyword)
                          || (isStatus && tripInfo.Status == status))

                          let driverName = $"{driver.FirstName} {driver.LastName}"
                          let busboyName = $"{busBoy.FirstName} {busBoy.LastName}"
                          select new TripManagementViewModel
                          {
                              Id = tripInfo.Id,
                              TripId = trip.Id,
                              //BaseFee = trip.BaseFee,
                              DepartureDate = tripInfo.DepartureDate.ToString(CoreConstants.DateFormat),
                              Driver = driverName.Trim(),
                              BusBoy = busboyName.Trim(),
                              Status = tripInfo.Status.GetDescription()
                          };

            return request.Paginate(pageStart, pageEnd).ToList();
        }
    }
}