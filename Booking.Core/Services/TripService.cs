using Booking.Core.Dtos;
using Booking.Core.Enums;
using Booking.Core.Helpers;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.Validators;
using Booking.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NPOI;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.Utils;
using Tornado.Shared.ViewModels;
using static System.DayOfWeek;

namespace Booking.Core.Services
{
    public class TripService : Service<Trip>, ITripService
    {
        private readonly IHttpUserService _currentUserService;
        private readonly IRouteService _routeService;

        public TripService(IUnitOfWork unitOfWork,
         IHttpUserService currentUserService,
          IRouteService routeService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
            _routeService = routeService;
        }

        public Task<List<TripSearchResponseViewModel>> SearchTrips(TripQueryViewModel searchModel, out int totalCount)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            var routes = UnitOfWork.Repository<Route>().GetAll();
            var vehicles = UnitOfWork.Repository<Vehicle>().GetAll();
            var excludedSeats = UnitOfWork.Repository<VehicleExcludedSeat>().GetAll();
            var reservation = UnitOfWork.Repository<Models.Booking>().GetAll();
            var tripSettings = UnitOfWork.Repository<TripDays>().GetAll();
            var tripFees = UnitOfWork.Repository<VehicleModelRouteFee>().GetAll();
            var routePoints = UnitOfWork.Repository<RoutePoint>();

            var vehicleModels = UnitOfWork.Repository<VehicleModel>().GetAll();
            var subrouteFees = UnitOfWork.Repository<SubRouteFee>().GetAll();

            var currentTime = new TimeSpan(Clock.Now.Hour, Clock.Now.Minute, 0);
            var day = Clock.Now.DayOfWeek;
            var currentDate = Clock.Now.Date;

            var trips = (from trip in GetAll()

                         join route in routes
                         on trip.RouteId equals route.Id

                         join departure in routePoints.GetAll()
                         on route.Id equals departure.RouteId

                         join destination in routePoints.GetAll()
                         on route.Id equals destination.RouteId

                         join fee in tripFees
                         on new
                         {
                             trip.RouteId,
                             trip.Vehicle.VehicleModelId
                         }
                         equals new
                         {
                             fee.RouteId,
                             fee.VehicleModelId
                         }

                         //join sf in subrouteFees
                         //on new
                         //{
                         //    DepartureId = departure.Id,
                         //    trip.Vehicle.VehicleModelId
                         //}
                         //equals new
                         //{
                         //    DepartureId = sf.DeparturePointId,
                         //    sf.VehicleModelId
                         //} into stgrp

                         //from subrouteFee in stgrp.DefaultIfEmpty()

                         join vehicle in vehicles on
                         trip.VehicleId equals vehicle.Id

                         join tripSetting in tripSettings
                         on trip.TripDaysId equals tripSetting.Id

                         where trip.IsActive &&
                         departure.OrderNo < destination.OrderNo &&

                           ((departure.PointType == PointType.Departure ||
                           departure.PointType == PointType.Departure_Destination) &&
                           departure.PointId == searchModel.DepartureId) &&

                          ((destination.PointType == PointType.Destination ||
                           destination.PointType == PointType.Departure_Destination) &&
                           destination.PointId == searchModel.DestinationId) &&

                          trip.DepartureTime >= currentTime &&

                          (tripSetting.Friday && day == Friday ||
                           tripSetting.Thursday && day == Thursday ||
                           tripSetting.Wednesday && day == Wednesday ||
                           tripSetting.Tuesday && day == Tuesday ||
                           tripSetting.Monday && day == Monday ||
                           tripSetting.Saturday && day == Saturday ||
                           tripSetting.Sunday && day == Sunday)

                         let bookedSeats = reservation //today's booked seats
                          .Where(x => x.TripManagement.TripId == trip.Id &&
                           x.TripManagement.DepartureDate.Date == currentDate &&
                           x.BookingStatus != BookingStatus.CANCELLED)
                          .Select(x => new BookedSeat
                          {
                              seatNumber = x.SeatNumber,
                              destinationOrderNumber = x.Destination.OrderNo
                          })

                         let noOfSeats = vehicle.VehicleModel.NoOfSeats

                         let excludedseats = excludedSeats
                          .Where(x => x.VehicleModelId == vehicle.VehicleModelId && x.IsActive)
                          .Select(x => x.SeatNumber)

                          let subrouteFee = subrouteFees.FirstOrDefault(p => p.DeparturePointId == departure.Id &&
                                                                                    p.VehicleModelId == vehicle.VehicleModelId)
                         select new TripSearchResponseViewModel
                         {
                             Id = trip.Id,
                             seatsInVehicle = noOfSeats,
                             RouteName = route.Name,
                             Departure = departure.Point.Title,
                             DepartureId = departure.Id,
                             DestinationId = destination.Id,
                             Destination = destination.Point.Title,
                             DepartureTime = trip.DepartureTime.ToTimeString(),
                             excludedSeats = excludedseats,
                             departureOrderNo = departure.OrderNo,
                             bookedSeats = bookedSeats,
                             Amount = (subrouteFee == null ? fee.BaseFee : subrouteFee.Fare),
                             DepartureDate = currentDate.ToString(CoreConstants.DateFormat),
                             VehicleModelTitle = vehicle.VehicleModel.Title
                         });

            totalCount = trips.Count();
            return trips.AsNoTracking().ToListAsync();
        }

        public Task<List<BusBoySearchTripsResponseViewModel>> GetAllTodaysBusBoyTrips(out int totalCount)
        {
            var userId = _currentUserService.GetCurrentUser().UserId;

            var existingBusBoy = UnitOfWork.Repository<BusBoy>().GetFirstOrDefault(p => p.UserId == userId);

            if (existingBusBoy == null)
            {
                throw new ArgumentNullException("Busboy is invalid");
            }

            var routes = UnitOfWork.Repository<Route>().GetAll();
            var vehicles = UnitOfWork.Repository<Vehicle>().GetAll();
            var excludedSeats = UnitOfWork.Repository<VehicleExcludedSeat>().GetAll();
            var reservation = UnitOfWork.Repository<Models.Booking>().GetAll();
            var tripSettings = UnitOfWork.Repository<TripDays>().GetAll();
            var tripFees = UnitOfWork.Repository<VehicleModelRouteFee>().GetAll();

            var vehicleModels = UnitOfWork.Repository<VehicleModel>().GetAll();

            var currentTime = new TimeSpan(Clock.Now.Hour, Clock.Now.Minute, 0);
            var day = Clock.Now.DayOfWeek;
            var currentDate = Clock.Now.Date;

            var trips = (from trip in GetAll()

                         join route in routes
                         on trip.RouteId equals route.Id

                         join fee in tripFees
                         on trip.RouteId equals fee.RouteId

                         join vehicle in vehicles on
                         trip.VehicleId equals vehicle.Id

                         join tripSetting in tripSettings
                         on trip.TripDaysId equals tripSetting.Id

                         where trip.IsActive &&

                          trip.DepartureTime >= currentTime &&

                          (tripSetting.Friday && day == Friday ||
                           tripSetting.Thursday && day == Thursday ||
                           tripSetting.Wednesday && day == Wednesday ||
                           tripSetting.Tuesday && day == Tuesday ||
                           tripSetting.Monday && day == Monday ||
                           tripSetting.Saturday && day == Saturday ||
                           tripSetting.Sunday && day == Sunday)

                           && trip.VehicleId == existingBusBoy.AttachedVehicleId
                         let noOfSeats = vehicle.VehicleModel.NoOfSeats

                         let excludedseats = excludedSeats
                          .Where(x => x.VehicleModelId == vehicle.VehicleModelId && x.IsActive)
                          .Select(x => x.SeatNumber)

                         orderby trip.DepartureTime ascending

                         select new BusBoySearchTripsResponseViewModel
                         {
                             Id = trip.Id,
                             seatsInVehicle = noOfSeats,
                             RouteName = route.Name,
                             DepartureTime = trip.DepartureTime.ToTimeString(),
                             excludedSeats = excludedseats,
                             BaseAmount = fee.BaseFee,
                             DepartureDate = currentDate.ToString(CoreConstants.DateFormat),
                             VehicleModelTitle = vehicle.VehicleModel.Title
                         });

            totalCount = trips.Count();
            return trips.AsNoTracking().ToListAsync();
        }
        public Task<List<TripSearchResponseViewModel>> SearchTripForBusBoy(TripQueryViewModel searchModel, out int totalCount)
        {
            if (searchModel is null)
            {
                throw new ArgumentNullException(nameof(searchModel));
            }

            var userId = _currentUserService.GetCurrentUser().UserId;

            var existingBusBoy = UnitOfWork.Repository<BusBoy>().GetFirstOrDefault(p => p.UserId == userId);

            if (existingBusBoy == null)
            {
                throw new ArgumentNullException("Busboy is invalid");
            }
            //get bus boy and find out if its the current user 

            var routes = UnitOfWork.Repository<Route>().GetAll();
            var vehicles = UnitOfWork.Repository<Vehicle>().GetAll();
            var excludedSeats = UnitOfWork.Repository<VehicleExcludedSeat>().GetAll();
            var reservation = UnitOfWork.Repository<Models.Booking>().GetAll();
            var tripSettings = UnitOfWork.Repository<TripDays>().GetAll();
            var tripFees = UnitOfWork.Repository<VehicleModelRouteFee>().GetAll();
            var routePoints = UnitOfWork.Repository<RoutePoint>();

            var vehicleModels = UnitOfWork.Repository<VehicleModel>().GetAll();
            var subrouteFees = UnitOfWork.Repository<SubRouteFee>().GetAll();

            var currentTime = new TimeSpan(Clock.Now.Hour, Clock.Now.Minute, 0);
            var day = Clock.Now.DayOfWeek;
            var currentDate = Clock.Now.Date;

            var trips = (from trip in GetAll()

                         join route in routes
                         on trip.RouteId equals route.Id

                         join departure in routePoints.GetAll()
                         on route.Id equals departure.RouteId

                         join destination in routePoints.GetAll()
                         on route.Id equals destination.RouteId

                         join fee in tripFees
                         on new
                         {
                             trip.RouteId,
                             trip.Vehicle.VehicleModelId
                         }
                         equals new
                         {
                             fee.RouteId,
                             fee.VehicleModelId
                         }

                         join sf in subrouteFees
                         on new
                         {
                             DepartureId = departure.Id,
                             trip.Vehicle.VehicleModelId
                         }
                         equals new
                         {
                             DepartureId = sf.DeparturePointId,
                             sf.VehicleModelId
                         } into stgrp

                         from subrouteFee in stgrp.DefaultIfEmpty()


                         join vehicle in vehicles on
                         trip.VehicleId equals vehicle.Id

                         join tripSetting in tripSettings
                         on trip.TripDaysId equals tripSetting.Id

                         where trip.IsActive &&
                         departure.OrderNo < destination.OrderNo &&

                           ((departure.PointType == PointType.Departure ||
                           departure.PointType == PointType.Departure_Destination) &&
                           departure.PointId == searchModel.DepartureId) &&

                          ((destination.PointType == PointType.Destination ||
                           destination.PointType == PointType.Departure_Destination) &&
                           destination.PointId == searchModel.DestinationId) &&

                          trip.DepartureTime >= currentTime &&

                          (tripSetting.Friday && day == Friday ||
                           tripSetting.Thursday && day == Thursday ||
                           tripSetting.Wednesday && day == Wednesday ||
                           tripSetting.Tuesday && day == Tuesday ||
                           tripSetting.Monday && day == Monday ||
                           tripSetting.Saturday && day == Saturday ||
                           tripSetting.Sunday && day == Sunday)

                           && trip.VehicleId == existingBusBoy.AttachedVehicleId
                         
                         let bookedSeats = reservation //today's booked seats
                          .Where(x => x.TripManagement.TripId == trip.Id &&
                           x.TripManagement.DepartureDate.Date == currentDate &&
                           x.BookingStatus != BookingStatus.CANCELLED)
                          .Select(x => new BookedSeat
                          {
                              seatNumber = x.SeatNumber,
                              destinationOrderNumber = x.Destination.OrderNo
                          })

                         let noOfSeats = vehicle.VehicleModel.NoOfSeats

                         let excludedseats = excludedSeats
                          .Where(x => x.VehicleModelId == vehicle.VehicleModelId && x.IsActive)
                          .Select(x => x.SeatNumber)
                         
                         orderby trip.DepartureTime ascending

                         select new TripSearchResponseViewModel
                         {
                             Id = trip.Id,
                             seatsInVehicle = noOfSeats,
                             RouteName = route.Name,
                             Departure = departure.Point.Title,
                             DepartureId = departure.Id,
                             DestinationId = destination.Id,
                             Destination = destination.Point.Title,
                             DepartureTime = trip.DepartureTime.ToTimeString(),
                             excludedSeats = excludedseats,
                             departureOrderNo = departure.OrderNo,
                             bookedSeats = bookedSeats,
                             Amount = (subrouteFee == null ? fee.BaseFee : subrouteFee.Fare),
                             DepartureDate = currentDate.ToString(CoreConstants.DateFormat),
                             VehicleModelTitle = vehicle.VehicleModel.Title
                         });

            totalCount = trips.Count();
            return trips.AsNoTracking().ToListAsync();
        }

        private string CreateTripName(string routeName, string vehicleModelTitle, string departureTime) => $"{routeName}/{vehicleModelTitle}/{departureTime}";

        public async Task<ApiResponse<TripViewModel>> CreateTrip(TripViewModel model)
        {
            //validate if a trip already has the same route and departure time
            var response = new ApiResponse<TripViewModel> { };
            string responseMessage = "";

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new CreateTripValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var routeinfomation = await _routeService.GetByIdAsync(model.RouteId);

            if (routeinfomation == null)
            {
                responseMessage = "Route ID does not exist";
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var canParseTime = DateTime.TryParse(model.DepartureTime, out DateTime time);

            var tripName = CreateTripName(routeinfomation.Name, model.VehicleModelTitle, model.DepartureTime);

            var existing = FirstOrDefault(p => p.Title == tripName);

            if (existing != null)
            {
                responseMessage = "Trip with name already exists";
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                response.Code = ApiResponseCodes.FAILED;
                goto ReturnToCaller;
            }
            if (!canParseTime)
            {
                responseMessage = "Cannot determine time from format";
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                goto ReturnToCaller;
            }

            Trip trip = new Trip
            {
                CreatedOn = Clock.Now,
                DepartureTime = new TimeSpan(time.Hour, time.Minute, 0),
                CanBeScheduled = model.CanBeScheduled,
                DiscountEndDate = model.DiscountEndDate,
                DiscountStartDate = model.DiscountStartDate,
                Title = tripName,
                VehicleId = model.VehicleId,
                Discount = model.Discount,
                RouteId = model.RouteId,
                CreatedBy = _currentUserService.GetCurrentUser().UserId,
                TripDaysId = model.TripDaysId,
                IsActive = true
            };

            bool isTripCreatedSuccessfully = await AddAsync(trip) > 0;

            response.Errors.Add(isTripCreatedSuccessfully ? "Successful" : "Could not add trip successfully");
            response.Code = isTripCreatedSuccessfully ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            ;

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<ViewTripViewModel>> DeleteTrip(DeleteTripViewModel model)
        {
            var response = new ApiResponse<ViewTripViewModel>();
            string responseMessage;

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new DeleteTripValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var existingTripModel = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingTripModel == null)
            {
                responseMessage = "Trip does not exist or has been deleted";
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            existingTripModel.IsDeleted = true;
            existingTripModel.ModifiedOn = Clock.Now;
            existingTripModel.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            Delete(existingTripModel);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = (ViewTripViewModel)existingTripModel;
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<ViewTripViewModel>> DeleteTripChildrenFee(DeleteTripChildrenFeeViewModel model)
        {
            var response = new ApiResponse<ViewTripViewModel>();
            string responseMessage;

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new DeleteTripChildrenFeeValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }


            var existingTripModel = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingTripModel == null)
            {
                response.Code = ApiResponseCodes.ERROR;
                response.Description = "Trip does not exist or has been deleted";
                goto ReturnToCaller;
            }

            existingTripModel.ModifiedOn = Clock.Now;
            existingTripModel.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            Update(existingTripModel);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = (ViewTripViewModel)existingTripModel;
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<ViewTripViewModel>> DeleteTripDiscount(DeleteTripDiscountViewModel model)
        {
            var response = new ApiResponse<ViewTripViewModel>();
            string responseMessage;
            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new DeleteTripDiscountValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }


            var existingTripModel = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingTripModel == null)
            {
                response.Code = ApiResponseCodes.ERROR;
                response.Description = "Trip does not exist or has been deleted";
                goto ReturnToCaller;
            }

            existingTripModel.Discount = 0;
            existingTripModel.DiscountStartDate = null;
            existingTripModel.DiscountEndDate = null;
            existingTripModel.ModifiedOn = Clock.Now;
            existingTripModel.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            Update(existingTripModel);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = (ViewTripViewModel)existingTripModel;
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<ViewTripViewModel>> GetTripById(GetTripByIdViewModel model)
        {
            var response = new ApiResponse<ViewTripViewModel>();
            string responseMessage;

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new GetTripsByIdValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            if (!Guid.TryParse(model.Id, out Guid tripId))
            {
                responseMessage = "Invalid trip id was specified";
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            //var tripData = await GetByIdAsync(tripId);
            var tripData = await Task.FromResult((from trip in UnitOfWork.Repository<Trip>().GetAll()
                                                  join tripday in UnitOfWork.Repository<TripDays>().GetAll()
                                                  on trip.TripDaysId equals tripday.Id

                                                  select new TripDto
                                                  {
                                                      Saturday = tripday.Saturday,
                                                      Sunday = tripday.Sunday,
                                                      Monday = tripday.Monday,
                                                      Tuesday = tripday.Tuesday,
                                                      Wednesday = tripday.Wednesday,

                                                      //handle object reference exception here
                                                      CanBeScheduled = trip.CanBeScheduled,
                                                      Friday = tripday.Friday,
                                                      DepartureTime = trip.DepartureTime.ToString(),
                                                      Id = trip.Id,
                                                      TripDaysId = tripday.Id,
                                                      TripDaysTitle = tripday.Title,
                                                      RouteId = trip.RouteId,
                                                      Thursday = tripday.Thursday,
                                                      Title = trip.Title,
                                                  }).FirstOrDefault()).ConfigureAwait(false);

            if (tripData == null)
            {
                responseMessage = "Trip does not exist";
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            response.TotalCount = tripData != null ? 1 : 0;
            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = (ViewTripViewModel)tripData;
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<ViewTripViewModel>> EditTrip(EditTripViewModel model)
        {
            var response = new ApiResponse<ViewTripViewModel>();
            string responseMessage;

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new EditTripValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var existingTripData = await GetByIdAsync(Guid.Parse(model.Id));
            if (existingTripData == null)
            {
                responseMessage = "Trip does not exist";
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var routeinfomation = await _routeService.GetByIdAsync(model.RouteId);

            if (routeinfomation == null)
            {
                responseMessage = "Route ID does not exist";
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var candidateTripName = CreateTripName(routeinfomation.Name, model.VehicleModelTitle, model.DepartureTime);

            bool hasRouteIdChanged = !candidateTripName.Equals(existingTripData.Title, StringComparison.InvariantCultureIgnoreCase);

            if (hasRouteIdChanged)
            {
                existingTripData.Title = candidateTripName;
            }

            // existingData.ModifiedBy = model. TODO: Add current user id that updated this record
            existingTripData.ModifiedOn = Clock.Now;
            existingTripData.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingTripData.CanBeScheduled = model.CanBeScheduled;
            existingTripData.Discount = model.Discount;
            existingTripData.DiscountStartDate = model.DiscountStartDate;
            existingTripData.DiscountEndDate = model.DiscountEndDate;
            existingTripData.VehicleId = model.VehicleId;
            existingTripData.LastTripCreationDate = model.LastTripCreationDate;
            existingTripData.TripDaysId = model.TripDaysId;
            existingTripData.IsActive = model.IsActive;

            var isVehicleUpdated = await UpdateAsync(existingTripData) > 0;

            response.Code = isVehicleUpdated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isVehicleUpdated ? "Successful" : $"Could not update {model.Title ?? "Trip"} at this time, please try again later";
            response.Payload = (ViewTripViewModel)existingTripData;
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }


        public async Task<ApiResponse<ViewTripViewModel>> EditTripDiscount(TripDiscountViewModel model)
        {
            var response = new ApiResponse<ViewTripViewModel>();
            string responseMessage;

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            var validationResult = new EditTripDiscountValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                responseMessage = validationResult.ToString(", ");
                response.Code = ApiResponseCodes.ERROR;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }


            var existingTripModel = await GetByIdAsync(Guid.Parse(model.Id));

            if (existingTripModel == null)
            {
                response.Code = ApiResponseCodes.ERROR;
                response.Description = "Trip does not exist or has been deleted";
                goto ReturnToCaller;
            }

            existingTripModel.Discount = model.Discount;
            existingTripModel.DiscountStartDate = model.DiscountStartDate;
            existingTripModel.DiscountEndDate = model.DiscountEndDate;
            existingTripModel.ModifiedOn = Clock.Now;
            existingTripModel.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            Update(existingTripModel);

            response.Code = ApiResponseCodes.OK;
            response.Description = "Successful";
            response.Payload = (ViewTripViewModel)existingTripModel;
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetAllTrip(GetAllTripViewModel model)
        {
            var response = new ApiResponse<PaginatedList<ViewTripViewModel>> { Code = ApiResponseCodes.OK };
            string responseMessage = "";

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            model.PageIndex ??= 0;
            model.PageTotal ??= 50;

            var validationResult = new GetAllTripValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }
            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Trip>().GetAll()
                        select new ViewTripViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            DiscountEndDate = dataModel.DiscountEndDate,
                            DiscountStartDate = dataModel.DiscountStartDate,
                            Discount = dataModel.Discount,
                            CanBeScheduled = dataModel.CanBeScheduled,
                            DepartureTime = dataModel.DepartureTime.ToString(),
                            Title = dataModel.Title,
                            VehicleId = dataModel.VehicleId ?? Guid.Empty,
                            Id = dataModel.Id.ToString()
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                       pageIndex: response.Payload.PageIndex,
                       pageSize: response.Payload.PageSize,
                       totalPageCount: response.Payload.TotalPageCount,
                       totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }


        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsByVehicleId(GetTripsByVehicleIdViewModel model)
        {
            var response = new ApiResponse<PaginatedList<ViewTripViewModel>> { Code = ApiResponseCodes.OK };
            string responseMessage = "";

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            model.PageIndex ??= 0;
            model.PageTotal ??= 50;


            var validationResult = new GetTripsByVehicleIdValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Trip>().GetAll()
                        where dataModel.VehicleId.Equals(model.VehicleId)
                        select new ViewTripViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            DiscountEndDate = dataModel.DiscountEndDate,
                            DiscountStartDate = dataModel.DiscountStartDate,
                            Discount = dataModel.Discount,
                            CanBeScheduled = dataModel.CanBeScheduled,
                            DepartureTime = dataModel.DepartureTime.ToString(),
                            Title = dataModel.Title,
                            VehicleId = dataModel.VehicleId ?? Guid.Empty,
                            Id = dataModel.Id.ToString()
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                   pageIndex: response.Payload.PageIndex,
                   pageSize: response.Payload.PageSize,
                   totalPageCount: response.Payload.TotalPageCount,
                   totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }


        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsByRegistrationNumber(GetTripsByVehicleRegNoViewModel model)
        {
            var response = new ApiResponse<PaginatedList<ViewTripViewModel>> { Code = ApiResponseCodes.OK };
            string responseMessage = "";

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            model.PageIndex ??= 0;
            model.PageTotal ??= 50;


            var validationResult = new GetTripsByVehicleRegNoValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Trip>().GetAll()
                        join vehicles in UnitOfWork.Repository<Vehicle>().GetAll()
                        on dataModel.VehicleId equals vehicles.Id
                        where vehicles.RegistrationNumber.ToLower().Contains(model.VehicleRegistrationNumber.ToLower())
                        select new ViewTripViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            DiscountEndDate = dataModel.DiscountEndDate,
                            DiscountStartDate = dataModel.DiscountStartDate,
                            Discount = dataModel.Discount,
                            CanBeScheduled = dataModel.CanBeScheduled,
                            DepartureTime = dataModel.DepartureTime.ToString(),
                            Title = dataModel.Title,
                            VehicleId = dataModel.VehicleId ?? Guid.Empty,
                            Id = dataModel.Id.ToString()
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                   pageIndex: response.Payload.PageIndex,
                   pageSize: response.Payload.PageSize,
                   totalPageCount: response.Payload.TotalPageCount,
                   totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }

        public async Task<ApiResponse<PaginatedList<ViewTripViewModel>>> GetTripsWithActiveDiscount(GetTripsWithActiveDiscountViewModel model)
        {
            var response = new ApiResponse<PaginatedList<ViewTripViewModel>> { Code = ApiResponseCodes.OK };
            string responseMessage = "";

            if (model == null)
            {
                responseMessage = "Model can not be empty";
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = responseMessage;
                response.Errors.Add(responseMessage);
                goto ReturnToCaller;
            }

            model.PageIndex ??= 0;
            model.PageTotal ??= 50;


            var validationResult = new GetTripsWithActiveDiscountValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }

            DateTime currentDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("West Africa Standard Time"));

            var data = await Task.Run(() =>
            {
                return (from dataModel in UnitOfWork.Repository<Trip>().GetAll()
                        where dataModel.DiscountEndDate.Value.Date > currentDate.Date
                        select new ViewTripViewModel
                        {
                            CreatedId = dataModel.CreatedBy ?? Guid.Empty,
                            DiscountEndDate = dataModel.DiscountEndDate,
                            DiscountStartDate = dataModel.DiscountStartDate,
                            Discount = dataModel.Discount,
                            CanBeScheduled = dataModel.CanBeScheduled,
                            DepartureTime = dataModel.DepartureTime.ToString(),
                            Title = dataModel.Title,
                            VehicleId = dataModel.VehicleId ?? Guid.Empty,
                            Id = dataModel.Id.ToString()
                        });
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                        pageIndex: response.Payload.PageIndex,
                        pageSize: response.Payload.PageSize,
                        totalPageCount: response.Payload.TotalPageCount,
                        totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No vehicle record found";
            response.ResponseCode = ResponseCodeHelper.OK.ToString();

        ReturnToCaller:
            return response;
        }
    }
}