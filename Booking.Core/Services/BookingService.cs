using Booking.Core.Enums;
using Booking.Core.Events;
using Booking.Core.Helpers;
using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using Booking.Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tornado.Shared.AspNetCore;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Helpers;
using Tornado.Shared.Timing;
using Tornado.Shared.Utils;
using static Booking.Core.Helpers.CoreConstants;
using static System.DayOfWeek;
using Model = Booking.Core.Models;

namespace Booking.Core.Services
{
    public class BookingService : Service<Model.Booking>, IBookingService
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IHttpUserService _currentUserService;
        private readonly ITripManagementService _tripmanagementService;
        private readonly IBookingEventService _bookingEventService;
        private readonly IVehicleExcludedSeatService _vehicleExcludedSeatService;
        private readonly IPickupPointService _pickupPointService;
        private readonly HttpClient _httpClient;
        private readonly GigLogisticsUrlConfig _giglUrls;
        private readonly GigMobilityUrlConfig _gigmUrls;

        private readonly IExternalBookingDetailsService _externalBookingDetailsService;


        public BookingService(
            IUnitOfWork unitOfWork,
            IHttpUserService currentUserService,
            IGuidGenerator guidGenerator,
            ITripManagementService tripmanagementService,
            IBookingEventService bookingEventService,
            IVehicleExcludedSeatService vehicleExcludedSeatService,
            IPickupPointService pickupPointService, HttpClient httpClient,
            IOptions<GigMobilityUrlConfig> gigmUrls,
            IOptions<GigLogisticsUrlConfig> giglUrls,
            IExternalBookingDetailsService externalBookingDetailsService) : base(unitOfWork)
        {
            _currentUserService = currentUserService;
            _guidGenerator = guidGenerator;
            _tripmanagementService = tripmanagementService;
            _bookingEventService = bookingEventService;
            _vehicleExcludedSeatService = vehicleExcludedSeatService;
            _pickupPointService = pickupPointService;
            _httpClient = httpClient;
            _giglUrls = giglUrls.Value;
            _externalBookingDetailsService = externalBookingDetailsService;
            _gigmUrls = gigmUrls.Value;
        }


        static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);

        public async Task<List<ValidationResult>> CheckinRefcode(string refcode)
        {
            var booking = FirstOrDefault(x => x.RefCode == refcode);

            if (booking is null || booking.BookingStatus == BookingStatus.CANCELLED)
            {
                results.Add(new ValidationResult("Booking not found"));
                goto completed;
            }

            if (booking.CheckInStatus != CheckInStatus.PENDING)
            {
                results.Add(new ValidationResult($"Status is {booking.CheckInStatus}"));
                goto completed;
            }

            booking.CheckInStatus = CheckInStatus.CHECKED_IN;
            booking.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            await UpdateAsync(booking);

        completed:
            return results;
        }

        public async Task<List<ValidationResult>> CheckoutRefCode(string refcode)
        {
            var booking = FirstOrDefault(x => x.RefCode == refcode);

            if (booking is null || booking.BookingStatus == BookingStatus.CANCELLED)
            {
                results.Add(new ValidationResult("Booking not found"));
                goto completed;
            }

            if (booking.CheckInStatus == CheckInStatus.CHECKED_OUT)
            {
                results.Add(new ValidationResult($"Refcode used previously"));
                goto completed;
            }

            booking.CheckInStatus = CheckInStatus.CHECKED_OUT;
            booking.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            await UpdateAsync(booking);

        completed:
            return results;
        }

        private async Task<PendingTripInformationViewModel> GetTripAvailableSeatsInfo(Trip trip, Guid departureId, Guid destinationId)
        {
            if (trip is null)
                throw new ArgumentNullException(nameof(trip));

            var tripInfos = UnitOfWork.Repository<TripManagement>().GetAll();
            var vehicles = UnitOfWork.Repository<Vehicle>();
            var excludedSeats = UnitOfWork.Repository<VehicleExcludedSeat>().GetAll();
            var reservation = UnitOfWork.Repository<Model.Booking>().GetAll();
            var tripSettings = UnitOfWork.Repository<TripDays>().GetAll();
            var routePoints = UnitOfWork.Repository<RoutePoint>();

            var currentTime = new TimeSpan(Clock.Now.Hour, Clock.Now.Minute, 0);
            var day = Clock.Now.DayOfWeek;
            var currentDate = Clock.Now.Date;

            var query = await (from tripInformation in tripInfos

                               join departure in routePoints.GetAll()
                               on trip.RouteId equals departure.RouteId

                               join destination in routePoints.GetAll()
                               on trip.RouteId equals destination.RouteId

                               join vehicle in vehicles.GetAll()
                               on trip.VehicleId equals vehicle.Id

                               join TripDays ts in tripSettings
                               on trip.TripDaysId equals ts.Id into tsGrp

                               //on trip.Id equals ts.TripId into tsGrp
                               from tripSetting in tsGrp.DefaultIfEmpty()

                               where trip.IsActive
                               && (departure.OrderNo < destination.OrderNo)//route validation
                               && ((departure.PointType == PointType.Departure ||
                               departure.PointType == PointType.Departure_Destination) &&
                               departure.Id == departureId) &&

                               ((destination.PointType == PointType.Destination ||
                               destination.PointType == PointType.Departure_Destination) &&
                               destination.Id == destinationId)

                               && tripInformation.TripId == trip.Id
                               && tripInformation.Status != TripManagementStatus.ENDED
                               && (tripInformation.DepartureDate == null ||
                                tripInformation.DepartureDate.Date == currentDate)

                               && trip.DepartureTime >= currentTime
                               && (tripSetting.Friday && day == Friday
                                || tripSetting.Thursday && day == Thursday
                                || tripSetting.Wednesday && day == Wednesday
                                || tripSetting.Tuesday && day == Tuesday
                                || tripSetting.Monday && day == Monday
                                || tripSetting.Saturday && day == Saturday
                                || tripSetting.Sunday && day == Sunday)

                               let excludedseats = excludedSeats
                               .Where(x => x.VehicleModelId == vehicle.VehicleModelId && x.IsActive).Select(x => x.SeatNumber)

                               let bookedSeats = reservation
                             .Where(x => x.TripManagementId == tripInformation.Id
                                    && x.BookingStatus != BookingStatus.CANCELLED)
                             .Select(x => new BookedSeat
                             {
                                 seatNumber = x.SeatNumber,
                                 destinationOrderNumber = x.Destination.OrderNo
                             })

                               let noOfSeats = vehicle.VehicleModel.NoOfSeats

                               select new PendingTripInformationViewModel
                               {
                                   Id = tripInformation.Id,
                                   TripId = trip.Id,
                                   DepartureTime = tripInformation.DepartureDate,
                                   Status = tripInformation.Status,
                                   seatsInVehicle = noOfSeats,
                                   bookedSeats = bookedSeats,
                                   excludedSeats = excludedseats.ToArray(),
                                   departureOrderNo = departure.OrderNo,
                                   DepartureRoutePointId = departure.Id,
                                   DestinationRoutePointId = destination.Id
                               }).ToListAsync().ConfigureAwait(false);

            return query.FirstOrDefault(x => x.AvailableSeats.Length > 0);
        }

        public decimal? ComputeteFare(Guid tripId, Guid departureId, Guid destinationId)
        {
            var routePoints = UnitOfWork.Repository<RoutePoint>();
            var subfees = UnitOfWork.Repository<SubRouteFee>().GetAll();
            var routeHikes = UnitOfWork.Repository<RouteHike>().GetAll();
            var tripFees = UnitOfWork.Repository<VehicleModelRouteFee>().GetAll();

            return (from trip in UnitOfWork.Repository<Trip>().GetAll()

                    join departure in routePoints.GetAll()
                          on trip.RouteId equals departure.RouteId

                    join destination in routePoints.GetAll()
                          on trip.RouteId equals destination.RouteId

                    join fee in tripFees
                          on trip.RouteId equals fee.RouteId

                    join sf in subfees
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

                    where (departure.OrderNo < destination.OrderNo)
                          && (departure.PointType == PointType.Departure ||
                          departure.PointType == PointType.Departure_Destination) &&
                          departure.Id == departureId &&

                          (destination.PointType == PointType.Destination ||
                          destination.PointType == PointType.Departure_Destination) &&
                          destination.Id == destinationId

                          && fee.VehicleModelId == trip.Vehicle.VehicleModelId
                    select new
                    {
                        Fare = subrouteFee == null ? fee.BaseFee : subrouteFee.Fare,
                    }).FirstOrDefault()?.Fare;
        }

        public async Task<(List<ValidationResult> validationResults, BookingCreateResponseModel bookingResult)> CreateBooking(BookingRequestModel model)
        {
            BookingCreateResponseModel bookingResponse = new BookingCreateResponseModel();

            if (!IsValid(model))
                goto completed;

            var trip = UnitOfWork.Repository<Trip>()
                                 .GetSingleIncluding(x => x.Id == model.TripId,
                                                        x => x.Vehicle,
                                                        x => x.Vehicle.VehicleModel,
                                                        x => x.Route);

            if (trip is null || !trip.IsActive || trip.Vehicle is null)
            {
                results.Add(new ValidationResult("Trip notfound"));
                goto completed;
            }

            if (!model.Seats.Any(x => x < trip.Vehicle.VehicleModel.NoOfSeats || x > trip?.Vehicle.VehicleModel.NoOfSeats))
            {
                results.Add(new ValidationResult("Selected seat(s) is not on the bus"));
                goto completed;
            }

            if (_vehicleExcludedSeatService.GetExcludedSeats(trip.Vehicle.VehicleModelId).Any(x => model.Seats.Contains(x)))
            {
                results.Add(new ValidationResult("Selected seat(s) cannot be booked"));
                goto completed;
            }

            await semaphoreSlim.WaitAsync();
            try
            {
                var pendingTripInfo = await GetTripAvailableSeatsInfo(trip, model.DepartureId, model.DestinationId);

                if (pendingTripInfo is null)
                {
                    results = _tripmanagementService.CreateTripManagement(new CreateTripManagementViewModel
                    {
                        TripId = trip.Id,
                        DepartureDate = Clock.Now.Date
                                             .AddHours(trip.DepartureTime.Hours)
                                             .AddMinutes(trip.DepartureTime.Minutes)
                    });

                    if (results.Any())
                        goto completed;

                    pendingTripInfo = await GetTripAvailableSeatsInfo(trip, model.DepartureId, model.DestinationId);

                    if (pendingTripInfo is null)
                    {
                        results.Add(new ValidationResult("Invalid routes selection"));
                        goto completed;
                    }
                }

                if (model.Seats.Any(x => !pendingTripInfo.AvailableSeats.Contains(x)))
                {
                    results.Add(new ValidationResult("Selected seat(s) already booked"));
                    goto completed;
                }

                var currentUser = _currentUserService.GetCurrentUser();

                var seatFee = ComputeteFare(trip.Id, model.DepartureId, model.DestinationId) ?? 0;

                var mainbooking = new Model.Booking
                {
                    TripManagementId = pendingTripInfo.Id,
                    SeatNumber = model.Seats[0],
                    BookingStatus = BookingStatus.PENDING,
                    BookingType = BookingType.Hire,
                    PaymentStatus = PaymentStatus.PENDING,
                    BookingPhoneNumber = $"{currentUser.DialingCode}{currentUser.PhoneNumber}",
                    BookingFirstName = currentUser.FirstName,
                    BookingLastName = currentUser.LastName,
                    CustomerId = currentUser.UserId,
                    CheckInStatus = CheckInStatus.PENDING,
                    BookingEmail = currentUser.Email,
                    DepartureId = pendingTripInfo.DepartureRoutePointId,
                    DestinationId = pendingTripInfo.DestinationRoutePointId,
                    Amount = seatFee,
                    RefCode = GenerateUniqueRefCode(),
                    CreatedBy = _currentUserService.GetCurrentUser().UserId,
                    IsMainBooking = true
                };

                Add(mainbooking);
                bookingResponse.SeatRefCode.Add(new SeatRefcodeModel
                {
                    Refcode = mainbooking.RefCode,
                    Seat = mainbooking.SeatNumber,
                });

                if (model.Seats.Length > 1)
                {
                    for (int i = 1; i < model.Seats.Length; i++)
                    {
                        var booking = new Model.Booking
                        {
                            TripManagementId = pendingTripInfo.Id,
                            SeatNumber = model.Seats[i],
                            BookingStatus = BookingStatus.PENDING,
                            BookingType = BookingType.Hire,
                            PaymentStatus = PaymentStatus.PENDING,
                            BookingPhoneNumber = currentUser.PhoneNumber,
                            BookingFirstName = currentUser.FirstName,
                            BookingLastName = currentUser.LastName,
                            CustomerId = currentUser.UserId,
                            CheckInStatus = CheckInStatus.PENDING,
                            BookingEmail = currentUser.Email,
                            DepartureId = pendingTripInfo.DepartureRoutePointId,
                            DestinationId = pendingTripInfo.DestinationRoutePointId,
                            RefCode = $"{mainbooking.RefCode}-{i}",
                            MainBookingRefCode = mainbooking.RefCode,
                            CreatedBy = currentUser.UserId
                        };

                        Add(booking);
                        bookingResponse.SeatRefCode.Add(new SeatRefcodeModel
                        {
                            Refcode = booking.RefCode,
                            Seat = booking.SeatNumber,
                        });

                        mainbooking.Amount += seatFee;

                        bookingResponse.SelectedSeats.Add(model.Seats[i]);
                    }

                    Update(mainbooking);
                }

                bookingResponse.BookingDate = mainbooking.CreatedOn.ToString(CoreConstants.DateFormat);
                bookingResponse.PaymentStatus = mainbooking.PaymentStatus.GetDescription();
                bookingResponse.BookingStatus = mainbooking.BookingStatus.GetDescription();
                bookingResponse.AmountPayble = mainbooking.Amount;

                await _bookingEventService.PublishAndLogEvent(new BookingCreatedIntegrationEvent(
                 refCode: mainbooking.RefCode,
                 departurePointId: $"{mainbooking.Departure?.Point.Id}",
                 departurePoint: mainbooking.Departure?.Point.Title,
                 departureDate: mainbooking.TripManagement?.DepartureDate.ToString(CoreConstants.DateFormat),
                 departureTime: mainbooking.TripManagement?.DepartureDate.ToString(CoreConstants.TimeFormat),
                 seatNo: $"{mainbooking.SeatNumber}",
                 customerName: $"{mainbooking.BookingFirstName} {mainbooking.BookingLastName}",
                 customerEmail: mainbooking.BookingEmail,
                 customerPhoneNumber: mainbooking.BookingPhoneNumber,
                 destinationPoint: mainbooking.Destination?.Point.Title,
                 tripId: $"{trip.Id}",
                 tripName: trip.Title,
                 routeName: trip?.Route?.Name,
                 routeId: trip?.Route?.Id.ToString(),
                 busboyId: "", //Bus boy information not available for customer initiated booking
                 busboyUsername: "", //Bus boy information not available for customer initiated booking
                 vehicleId: trip?.Vehicle?.Id.ToString(),
                 vehicleChassisNumber: trip?.Vehicle?.ChassisNumber,
                 vehicleRegistrationNumber: trip?.Vehicle?.RegistrationNumber,
                 destinationPointId: mainbooking?.DestinationId.ToString(),
                 amount: mainbooking.Amount,
                 paymentMethod: Enum.GetName(typeof(PaymentMethod), mainbooking.PaymentMethod),
                 paymentStatus: Enum.GetName(typeof(PaymentStatus), mainbooking.PaymentStatus)
         ));
            }
            finally
            {
                semaphoreSlim.Release();
            }

        completed:
            return (results, bookingResponse);
        }

        public async Task<(List<ValidationResult> validationResults, BookingCreateResponseModel bookingResult)> CreateBusBoyBooking(BusBoyBookingRequestModel model)
        {
            BookingCreateResponseModel bookingResponse = new BookingCreateResponseModel();

            if (!IsValid(model))
                goto completed;

            var trip = UnitOfWork.Repository<Trip>()
                                 .GetSingleIncluding(x => x.Id == model.TripId,
                                                        x => x.Vehicle,
                                                        x => x.Vehicle.VehicleModel,
                                                        x => x.Route);

            if (trip is null || !trip.IsActive || trip.Vehicle is null)
            {
                results.Add(new ValidationResult("Trip notfound"));
                goto completed;
            }

            if (!model.Seats.Any(x => x < trip.Vehicle.VehicleModel.NoOfSeats || x > trip.Vehicle.VehicleModel.NoOfSeats))
            {
                results.Add(new ValidationResult("Selected seat(s) is not on the bus"));
                goto completed;
            }

            if (_vehicleExcludedSeatService.GetExcludedSeats(trip.Vehicle.VehicleModelId).Any(x => model.Seats.Contains(x)))
            {
                results.Add(new ValidationResult("Selected seat(s) cannot be booked"));
                goto completed;
            }

            await semaphoreSlim.WaitAsync();
            try
            {
                var pendingTripInfo = await GetTripAvailableSeatsInfo(trip, model.DepartureId, model.DestinationId);

                if (pendingTripInfo is null)
                {
                    results = _tripmanagementService.CreateTripManagement(new CreateTripManagementViewModel
                    {
                        TripId = trip.Id,
                        DepartureDate = Clock.Now.Date
                                             .AddHours(trip.DepartureTime.Hours)
                                             .AddMinutes(trip.DepartureTime.Minutes)
                    });

                    if (results.Any())
                        goto completed;

                    pendingTripInfo = await GetTripAvailableSeatsInfo(trip, model.DepartureId, model.DestinationId);

                    if (pendingTripInfo is null)
                    {
                        results.Add(new ValidationResult("Invalid routes selection"));
                        goto completed;
                    }
                }

                if (model.Seats.Any(x => !pendingTripInfo.AvailableSeats.Contains(x)))
                {
                    results.Add(new ValidationResult("Selected seat(s) already booked"));
                    goto completed;
                }

                var currentUser = _currentUserService.GetCurrentUser();

                var seatFee = ComputeteFare(trip.Id, model.DepartureId, model.DestinationId) ?? 0;

                var mainbooking = new Model.Booking
                {
                    IsMainBooking = true,
                    TripManagementId = pendingTripInfo.Id,
                    SeatNumber = model.Seats[0],
                    BookingStatus = BookingStatus.APPROVED,
                    BookingType = BookingType.Hire,
                    PaymentStatus = PaymentStatus.PAID,
                    PaymentMethod = model.PaymentMethod == BusBoyPaymentMethod.CASH ? PaymentMethod.CASH :
                                    model.PaymentMethod == BusBoyPaymentMethod.POS ? PaymentMethod.POS : PaymentMethod.ON_ARRIVAL,
                    BookingPhoneNumber = $"{model.PhoneNumber}",
                    BookingFirstName = model.FirstName,
                    BookingLastName = model.LastName,
                    CheckInStatus = CheckInStatus.PENDING,
                    DepartureId = pendingTripInfo.DepartureRoutePointId,
                    DestinationId = pendingTripInfo.DestinationRoutePointId,

                    Amount = seatFee,

                    RefCode = GenerateUniqueRefCode(),
                    CreatedBy = _currentUserService.GetCurrentUser().UserId
                };

                Add(mainbooking);
                bookingResponse.SeatRefCode.Add(new SeatRefcodeModel
                {
                    Refcode = mainbooking.RefCode,
                    Seat = mainbooking.SeatNumber,
                });

                if (model.Seats.Length > 1)
                {
                    for (int i = 1; i < model.Seats.Length; i++)
                    {
                        var booking = new Model.Booking
                        {
                            TripManagementId = pendingTripInfo.Id,
                            SeatNumber = model.Seats[i],
                            BookingStatus = BookingStatus.APPROVED,
                            BookingType = BookingType.Hire,
                            PaymentStatus = PaymentStatus.PAID,
                            PaymentMethod = model.PaymentMethod == BusBoyPaymentMethod.CASH ? PaymentMethod.CASH :
                                    model.PaymentMethod == BusBoyPaymentMethod.POS ? PaymentMethod.POS : PaymentMethod.ON_ARRIVAL,
                            BookingPhoneNumber = model.PhoneNumber,
                            BookingFirstName = model.FirstName,
                            BookingLastName = model.LastName,
                            CheckInStatus = CheckInStatus.PENDING,
                            DepartureId = pendingTripInfo.DepartureRoutePointId,
                            DestinationId = pendingTripInfo.DestinationRoutePointId,
                            RefCode = $"{mainbooking.RefCode}-{i}",
                            MainBookingRefCode = mainbooking.RefCode,
                            CreatedBy = currentUser.UserId
                        };

                        Add(booking);
                        bookingResponse.SeatRefCode.Add(new SeatRefcodeModel
                        {
                            Refcode = booking.RefCode,
                            Seat = booking.SeatNumber,
                        });

                        mainbooking.Amount += seatFee;//Main booking carries payment

                        bookingResponse.SelectedSeats.Add(model.Seats[i]);
                    }

                    Update(mainbooking);
                }

                bookingResponse.BookingDate = mainbooking.CreatedOn.ToString(CoreConstants.DateFormat);
                bookingResponse.PaymentStatus = mainbooking.PaymentStatus.GetDescription();
                bookingResponse.BookingStatus = mainbooking.BookingStatus.GetDescription();
                bookingResponse.AmountPayble = mainbooking.Amount;

                await _bookingEventService.PublishAndLogEvent(new BookingCreatedIntegrationEvent(
                      refCode: mainbooking.RefCode,
                      departurePointId: $"{mainbooking?.Departure?.Point.Id}",
                      departurePoint: mainbooking?.Departure.Point.Title,
                      departureDate: mainbooking?.TripManagement?.DepartureDate.ToString(CoreConstants.DateFormat),
                      departureTime: mainbooking?.TripManagement?.DepartureDate.ToString(CoreConstants.TimeFormat),
                      seatNo: $"{mainbooking.SeatNumber}",
                      customerName: $"{mainbooking.BookingFirstName} {mainbooking.BookingLastName}",
                      customerEmail: mainbooking.BookingEmail,
                      customerPhoneNumber: mainbooking.BookingPhoneNumber,
                      destinationPoint: mainbooking?.Destination.Point.Title,
                      tripId: $"{trip.Id}",
                      tripName: trip.Title,
                      routeName: trip?.Route?.Name,
                      routeId: trip.Route?.Id.ToString(),
                      busboyId: mainbooking.TripManagement?.BusBoy.Id.ToString(),
                      busboyUsername: $"{mainbooking.TripManagement?.BusBoy.FirstName} {mainbooking.TripManagement?.BusBoy.LastName}",
                      vehicleId: trip?.Vehicle?.Id.ToString(),
                      vehicleChassisNumber: trip?.Vehicle?.ChassisNumber,
                      vehicleRegistrationNumber: trip?.Vehicle?.RegistrationNumber,
                      destinationPointId: mainbooking?.DestinationId.ToString(),
                      amount: mainbooking.Amount,
                      paymentMethod: Enum.GetName(typeof(PaymentMethod), mainbooking.PaymentMethod),
                      paymentStatus: Enum.GetName(typeof(PaymentStatus), mainbooking.PaymentStatus)
              ));
            }
            finally
            {
                semaphoreSlim.Release();
            }

        completed:
            return (results, bookingResponse);
        }


        public async Task<BookingDetailsModel> GetBookingByRefCode(string refCode)
        {
            var routePoints = UnitOfWork.Repository<RoutePoint>().GetAll();
            var trips = UnitOfWork.Repository<Trip>().GetAll();

            var query =
                from booking in GetAll()

                let departure = routePoints.FirstOrDefault(x => x.Id == booking.DepartureId)
                let destination = routePoints.FirstOrDefault(x => x.Id == booking.DestinationId)

                where booking.RefCode == refCode
                select new BookingDetailsModel
                {
                    RefCode = booking.RefCode,
                    Status = booking.BookingStatus.GetDescription(),
                    Amount = booking.Amount,
                    SeatNumber = booking.SeatNumber,
                    DateBooked = booking.CreatedOn.ToString(CoreConstants.DateFormat),
                    Departure = departure.Point.Title,
                    Destination = destination.Point.Title,
                    PaymentStatus = booking.PaymentStatus.GetDescription(),
                    DepartureDate = booking.TripManagement.DepartureDate.ToString(CoreConstants.DateFormat),
                    DepartureTime = booking.TripManagement.DepartureDate.ToString(CoreConstants.TimeFormat),
                };

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<string>> GetChildSeats(string refCode)
        {
            var seats = await (
                 from booking in GetAll()

                 where booking.MainBookingRefCode == refCode
                 select booking.SeatNumber)
                 .Cast<string>()
                 .ToListAsync();

            return seats;// string.Join(",", );
        }

        public async Task<EntityBookingDetailsModel> GetEntityBookingByRefCode(string refCode)
        {
            var tripRoutes = UnitOfWork.Repository<RoutePoint>().GetAll();
            var trips = UnitOfWork.Repository<Trip>().GetAll();

            var customerBooking = await
                (from booking in GetAll()

                 let departure = tripRoutes.FirstOrDefault(x => x.Id == booking.DepartureId)
                 let destination = tripRoutes.FirstOrDefault(x => x.Id == booking.DestinationId)

                 let additionalSeats = GetAll()
                 .Where(x => x.MainBookingRefCode == refCode)
                 .Select(x => $"{x.SeatNumber}")

                 where booking.RefCode == refCode
                 select new EntityBookingDetailsModel
                 {
                     Id = booking.Id,
                     RefCode = booking.RefCode,
                     Status = booking.BookingStatus,
                     Amount = booking.Amount,
                     MainSeatNumber = booking.SeatNumber.ToString(),
                     DateBooked = booking.CreatedOn,
                     Departure = departure.Point.Title,
                     Destination = destination.Point.Title,
                     PaymentStatus = booking.PaymentStatus,
                     CustomerId = booking.CustomerId ?? Guid.Empty,
                     CustomerEmail = booking.BookingEmail,
                     additionalSeats = additionalSeats
                 }).FirstOrDefaultAsync();

            return customerBooking;
        }

        public Task<List<BookingDetailsModel>> GetCustomerBookingsByCustomer(CustomerBookingSearchModel search, out int totalCount)
        {
            var routes = UnitOfWork.Repository<Route>().GetAll();
            var tripRoutes = UnitOfWork.Repository<RoutePoint>().GetAll();
            var trips = UnitOfWork.Repository<Trip>();
            var tripManagements = UnitOfWork.Repository<TripManagement>();

            var query =
                from booking in GetAll()

                join tripMgt in tripManagements.GetAll()
                on booking.TripManagementId equals tripMgt.Id

                join trip in trips.GetAll()
                on tripMgt.TripId equals trip.Id

                let departure = tripRoutes.FirstOrDefault(x => x.Id == booking.DepartureId)
                let destination = tripRoutes.FirstOrDefault(x => x.Id == booking.DestinationId)

                where booking.CustomerId == search.CustomerId && booking.IsMainBooking
                orderby booking.CreatedOn descending

                select new BookingDetailsModel
                {
                    DepartureDate = tripMgt.DepartureDate.ToString(CoreConstants.DateFormat),
                    DepartureTime = trip.DepartureTime.ToTimeString(),
                    RefCode = booking.RefCode,
                    Status = booking.BookingStatus.GetDescription(),
                    Amount = booking.Amount,
                    SeatNumber = booking.SeatNumber,
                    DateBooked = booking.CreatedOn.ToString(CoreConstants.DateFormat),
                    Departure = departure.Point.Title,
                    Destination = destination.Point.Title,
                    PaymentStatus = booking.PaymentStatus.GetDescription()
                };

            totalCount = query.Count();

            return query.ToListAsync();
        }

        public Task<List<BookingDetailsModel>> GetCustomerBookingsByPhoneNumber(PhoneNumberBookingSearchModel search, out int totalCount)
        {
            var routes = UnitOfWork.Repository<Route>().GetAll();
            var tripRoutes = UnitOfWork.Repository<RoutePoint>().GetAll();
            var trips = UnitOfWork.Repository<Trip>();
            var tripManagements = UnitOfWork.Repository<TripManagement>();

            var query =
                from booking in GetAll()

                join tripMgt in tripManagements.GetAll()
                on booking.TripManagementId equals tripMgt.Id

                join trip in trips.GetAll()
                on tripMgt.TripId equals trip.Id

                let departure = tripRoutes.FirstOrDefault(x => x.Id == booking.DepartureId)
                let destination = tripRoutes.FirstOrDefault(x => x.Id == booking.DestinationId)

                where booking.BookingPhoneNumber == search.PhoneNumber
                orderby booking.CreatedOn descending

                select new BookingDetailsModel
                {
                    DepartureDate = tripMgt.DepartureDate.ToString(CoreConstants.DateFormat),
                    DepartureTime = trip.DepartureTime.ToTimeString(),
                    RefCode = booking.RefCode,
                    Status = booking.BookingStatus.GetDescription(),
                    Amount = booking.Amount,
                    SeatNumber = booking.SeatNumber,
                    DateBooked = booking.CreatedOn.ToString(CoreConstants.DateFormat),
                    Departure = departure.Point.Title,
                    Destination = destination.Point.Title,
                    PaymentStatus = booking.PaymentStatus.GetDescription()
                };

            totalCount = query.Count();

            return query.ToListAsync();
        }

        public string GenerateUniqueRefCode()
        {
            var isUnique = false;
            string otp = string.Empty;

            while (!isUnique)
            {
                var bookingCodes = GetAll()
                     .Where(x => x.BookingStatus != BookingStatus.CANCELLED)
                     .Select(x => x.RefCode).ToList();

                otp = _guidGenerator.Create().ToString().Remove(8).ToUpper();

                isUnique = !bookingCodes.Any(a => a.Equals(otp));
            }

            return otp;
        }


        public async Task<List<ValidationResult>> UpdateBookingOnSuccessfulPaymentEvent(PaymentSucceededIntegrationEvent model)
        {
            var existingBooking = FirstOrDefault(x => x.RefCode == model.PaymentRefCode);

            if (existingBooking is null)
            {
                results.Add(new ValidationResult($"Booking:#{existingBooking.RefCode} NotFound for payment update."));
                return results;
            }

            if (existingBooking.PaymentStatus == PaymentStatus.PAID)
            {
                results.Add(new ValidationResult($"Booking#{existingBooking.RefCode} ==> Payment already exist."));
                return results;
            }

            if (existingBooking.Amount <= model.Amount)
            {
                UnitOfWork.BeginTransaction();

                existingBooking.ModifiedBy = model.CustomerId;

                existingBooking.BookingStatus = BookingStatus.APPROVED;
                existingBooking.PaymentStatus = PaymentStatus.PAID;
                existingBooking.PaymentMethod = PaymentMethod.WALLET;

                if (existingBooking.IsMainBooking)
                {
                    var childBookings = GetAll().Where(x => x.MainBookingRefCode == existingBooking.RefCode).ToList();
                    childBookings.ForEach((e) =>
                    {
                        e.ModifiedBy = model.CustomerId;

                        e.BookingStatus = BookingStatus.APPROVED;
                        e.PaymentMethod = PaymentMethod.WALLET;
                        e.PaymentStatus = PaymentStatus.PAID;
                    });
                }

                await UnitOfWork.CommitAsync();
            }

            return results;
        }

        public async Task<List<ValidationResult>> CancelBooking(CancelBookingModel model)
        {
            var booking = UnitOfWork.Repository<Model.Booking>()
                          .GetSingleIncluding(x => x.RefCode == model.Refcode,
                                                                x => x.TripManagement,
                                                                x => x.Destination.Point,
                                                                x => x.Departure.Point,
                                                                x => x.TripManagement.Trip);

            if (booking is null)
            {
                results.Add(new ValidationResult("Booking does not exist."));
                goto completed;
            }

            if (booking.CheckInStatus == CheckInStatus.CHECKED_IN || booking.CheckInStatus == CheckInStatus.CHECKED_OUT)
            {
                results.Add(new ValidationResult("Booking already checked-in and can no longer be cancelled"));
                goto completed;
            }

            if (booking.BookingStatus == BookingStatus.CANCELLED)
            {
                results.Add(new ValidationResult("Booking already cancelled"));
                goto completed;
            }

            booking.BookingStatus = BookingStatus.CANCELLED;
            booking.ModifiedBy = _currentUserService.GetCurrentUser().UserId;

            await UpdateAsync(booking);

            await _bookingEventService.PublishAndLogEvent(new BookingCancelledIntegrationEvent(
                booking.RefCode, booking?.TripManagement?.DepartureDate.ToString(CoreConstants.DateFormat),
                booking?.TripManagement?.DepartureDate.ToString(CoreConstants.TimeFormat),
                $"{booking.SeatNumber}", booking.BookingEmail,
                booking?.Departure?.Point?.Title, booking?.Destination?.Point?.Title,
                booking?.TripManagement?.Trip.Title, booking.BookingPhoneNumber,
                booking.CreatedOn.ToString(CoreConstants.DateFormat)
                ));

            goto completed;

        completed: return results;
        }

        private async Task<GigBookingDetails> VerifyGIGLRefcode(UpdateBookingByExternalRefViewModel refcodeModel)
        {

            if (string.IsNullOrEmpty(refcodeModel.RefCode))
            {
                return null;
            }

            var extBookingDetails = new GigBookingDetails();

            var getExternalRefCode = await _httpClient.GetExternalUriAsync
                <ExternalGIGLRefCodeDetailsViewModel>(_giglUrls.Base +
                GigLogisticsUrlConfig.GetWayBill(refcodeModel.GIGRefCode));

            if (getExternalRefCode.Code != HttpStatusCode.OK)
            {
                throw new Exception("Sevice unavailable.");
            }

            var externalData = getExternalRefCode?.Object ?? throw new Exception("returns empty");

            if (!string.Equals(externalData.CustomerEmail,
                _currentUserService.GetCurrentUser().Email, StringComparison.OrdinalIgnoreCase) ||
                !string.Equals(externalData.CustomerNumber, _currentUserService.GetCurrentUser()
                .PhoneNumber))
            {
                throw new Exception("Invalid user.");
            }

            extBookingDetails.Channel = GIGBookingChannel.GIGL;
            extBookingDetails.CreatedOn = Clock.Now;
            extBookingDetails.CustomerEmail = _currentUserService.GetCurrentUser().Email;
            extBookingDetails.CustomerPhoneNumber = _currentUserService.GetCurrentUser().PhoneNumber;
            extBookingDetails.CreatedBy = _currentUserService.GetCurrentUser().UserId;
            extBookingDetails.GIGRefCode = refcodeModel.GIGRefCode;
            extBookingDetails.GIGRefCodeStatus = GIGRefCodeStatus.PENDING;

            await _externalBookingDetailsService.AddAsync(extBookingDetails);

            return extBookingDetails;
        }


        private async Task<GigBookingDetails> VerifyGIGMobilityRefcode(UpdateBookingByExternalRefViewModel refcodeModel)
        {

            if (string.IsNullOrEmpty(refcodeModel.RefCode))
            {
                return null;
            }

            var extBookingDetails = new GigBookingDetails();

            var getExternalRefCode = await _httpClient.GetExternalUriAsync
                <ExternalGIGMRefCodeDetailsViewModel>(_giglUrls.Base +
                GigMobilityUrlConfig.GetRefCodeDetails(refcodeModel.GIGRefCode));

            if (getExternalRefCode.Code != HttpStatusCode.OK)
            {
                throw new Exception("Sevice unavailable.");
            }

            var externalData = getExternalRefCode?.Object ?? throw new Exception("returns empty");


            if (!string.Equals(externalData.PhoneNumber,
                _currentUserService.GetCurrentUser().PhoneNumber, StringComparison.OrdinalIgnoreCase))
            {
                throw new Exception("Invalid user.");
            }

            extBookingDetails.Channel = GIGBookingChannel.GIGM;
            extBookingDetails.CreatedOn = Clock.Now;
            extBookingDetails.CustomerEmail = _currentUserService.GetCurrentUser().Email;
            extBookingDetails.CustomerPhoneNumber = _currentUserService.GetCurrentUser().PhoneNumber;
            extBookingDetails.CreatedBy = _currentUserService.GetCurrentUser().UserId;
            extBookingDetails.GIGRefCode = refcodeModel.GIGRefCode;
            extBookingDetails.GIGRefCodeStatus = GIGRefCodeStatus.PENDING;

            await _externalBookingDetailsService.AddAsync(extBookingDetails);

            return extBookingDetails;
        }

        public async Task<List<ValidationResult>> BookSeatsViaExternalRefAsync(UpdateBookingByExternalRefViewModel model)
        {

            var externalRefCode = ExternalRefType(model.Channel);
            var extBookingDetails = GigBookingDetails(model.GIGRefCode);

            UnitOfWork.BeginTransaction();

            if (extBookingDetails == null)
            {
                switch (externalRefCode)
                {
                    case GIGBookingChannel.GIGL:
                        extBookingDetails = await VerifyGIGLRefcode(model);
                        break;

                    case GIGBookingChannel.GIGM:
                        extBookingDetails = await VerifyGIGLRefcode(model);
                        break;

                    default:
                        results.Add(new ValidationResult("Invalid GIG reference code."));
                        goto ReturnToCaller;
                }

            }

            if (extBookingDetails == null)
            {
                results.Add(new ValidationResult("Refcode is invalid or has already been used"));
                goto ReturnToCaller;
            }
            var userId = _currentUserService.GetCurrentUser().UserId;

            if (extBookingDetails.CreatedBy != userId)
            {
                results.Add(new ValidationResult("Invalid user"));
                goto ReturnToCaller;
            }
            var existingSeat = FirstOrDefault(seat => seat.RefCode == model.RefCode
                                && seat.BookingStatus == BookingStatus.PENDING
                                && Guid.Equals(seat.CustomerId, userId));

            existingSeat.GIGRefCode = model.GIGRefCode;
            existingSeat.BookingStatus = BookingStatus.APPROVED;
            existingSeat.BookingType = BookingType.GIG_BOOk;
            existingSeat.PaymentMethod = PaymentMethod.GIG_PAY;
            existingSeat.ModifiedBy = _currentUserService.GetCurrentUser().UserId;
            existingSeat.ModifiedOn = Clock.Now;

            extBookingDetails.GIGRefCodeStatus = GIGRefCodeStatus.USED;

            await UnitOfWork.CommitAsync();


        ReturnToCaller:
            return results;
        }
        private GigBookingDetails GigBookingDetails(string gigRefCode)
        {
            var existingRefCode =
               UnitOfWork.Repository<GigBookingDetails>()
               .GetFirstOrDefault(p => p.GIGRefCode == gigRefCode);

            switch (existingRefCode)
            {
                case null:
                    goto finish;

                case var invalidRef when (existingRefCode.GIGRefCodeStatus == GIGRefCodeStatus.INVALID) ||
                            (existingRefCode.GIGRefCodeStatus == GIGRefCodeStatus.USED):
                    throw new Exception("Invalid/Used Ref code");

                case var pendingRef when pendingRef.GIGRefCodeStatus == GIGRefCodeStatus.PENDING:
                    goto finish;
            }
        finish:
            return existingRefCode;
        }

        private string ExternalRefType(string extRefCode) => extRefCode switch
        {
            var code when code.StartsWith(GIGBookingChannel.GIGL, StringComparison.OrdinalIgnoreCase) => GIGBookingChannel.GIGL,
            var code when code.StartsWith(GIGBookingChannel.GIGM, StringComparison.OrdinalIgnoreCase) => GIGBookingChannel.GIGM,
            _ => throw new Exception("Unknown reference code")
        };
    }
}