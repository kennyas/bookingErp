using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Report.Core.Models;
using Report.Core.Services.Interfaces;
using Report.Core.Validators;
using Report.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Timing;
using Tornado.Shared.Utils;
using Tornado.Shared.ViewModels;

namespace Report.Core.Services
{
    public class CustomerBookingsReportService : Service<CustomerBookings>, ICustomerBookingsReportService
    {
        private readonly ILogger<CustomerBookingsReportService> _logger;

        public CustomerBookingsReportService(IUnitOfWork unitOfWork, ILogger<CustomerBookingsReportService> logger) : base(unitOfWork)
        {
            _logger = logger;
        }

        public async Task<ApiResponse<CustomerBookingsReportViewModel>> AddCustomerBookings(AddCustomerBookingsReportViewModel model)
        {
            var response = new ApiResponse<CustomerBookingsReportViewModel>();

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new AddCustomerBookingReportValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            var existingRecord = FirstOrDefault(d => string.Equals(d.RefCode, model.RefCode, StringComparison.OrdinalIgnoreCase));
            if (existingRecord != null)
            {
                response.Code = ApiResponseCodes.FAILED;
                response.Errors.Add("Record already exists");
                _logger.LogInformation("Failed to store booking details. Record already exists", model);
                goto ReturnToCaller;
            }

            var newCustomerBooking = new CustomerBookings
            {
                RefCode = model.RefCode,
                CustomerName = model.CustomerName,
                EmailAddress = model.EmailAddress,
                RouteId = model.RouteId,
                RouteName = model.RouteName,
                BookingStatus = model.BookingStatus,
                BusboyId = model.BusboyId,
                BusboyUsername = model.BusboyUsername,
                BookingDate = model.BookingDate,
                VehicleId = model.VehicleId,
                VehicleRegistrationNumber = model.VehicleRegistrationNumber,
                DepartureTerminalId = model.DepartureTerminalId,
                DepartureTerminalName = model.DepartureTerminalName,
                DestinationPickupPointId = model.DestinationPickupPointId,
                DestinationPickupPointName = model.DestinationPickupPointName,
                PaymentMethod = model.PaymentMethod,
                PaymentStatus = model.PaymentStatus,
                Amount = model.Amount,
                PhoneNumber = model.PhoneNumber,
                DepatureTime = model.DepartureTime,
                TripId = model.TripId,
                TripName = model.TripName,
                CreatedOn = Clock.Now,
                CreatedBy = Guid.Empty,
                DepartureDate = model.DepartureDate
            };

            await AddAsync(newCustomerBooking);

            response.Code = ApiResponseCodes.OK;
            response.Payload = (CustomerBookingsReportViewModel)newCustomerBooking;
            response.Description = $"Successfully saved booking details with RefCode ({model.RefCode})";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            _logger.LogInformation(response.Description ?? string.Join(", ", response.Errors), model);
            return response;
        }

        public async Task<ApiResponse<List<CustomerBookingsReportViewModel>>> AddCustomerBookings(List<AddCustomerBookingsReportViewModel> model)
        {
            var response = new ApiResponse<List<CustomerBookingsReportViewModel>>();

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            List<CustomerBookings> newCustomerBookingReportData = new List<CustomerBookings>();

            Guid createdBy = Guid.Empty;
            DateTime createdOn = Clock.Now;

            foreach (var tripSalesData in model)
            {
                newCustomerBookingReportData.Add(new CustomerBookings
                {
                    RefCode = tripSalesData.RefCode,
                    CustomerName = tripSalesData.CustomerName,
                    EmailAddress = tripSalesData.EmailAddress,
                    RouteId = tripSalesData.RouteId,
                    RouteName = tripSalesData.RouteName,
                    BookingStatus = tripSalesData.BookingStatus,
                    BusboyId = tripSalesData.BusboyId,
                    BusboyUsername = tripSalesData.BusboyUsername,
                    BookingDate = tripSalesData.BookingDate,
                    VehicleId = tripSalesData.VehicleId,
                    VehicleRegistrationNumber = tripSalesData.VehicleRegistrationNumber,
                    DepartureTerminalId = tripSalesData.DepartureTerminalId,
                    DepartureTerminalName = tripSalesData.DepartureTerminalName,
                    DestinationPickupPointId = tripSalesData.DestinationPickupPointId,
                    DestinationPickupPointName = tripSalesData.DestinationPickupPointName,
                    PaymentMethod = tripSalesData.PaymentMethod,
                    PaymentStatus = tripSalesData.PaymentStatus,
                    Amount = tripSalesData.Amount,
                    PhoneNumber = tripSalesData.PhoneNumber,
                    DepatureTime = tripSalesData.DepartureTime,
                    TripId = tripSalesData.TripId,
                    TripName = tripSalesData.TripName,
                    CreatedOn = createdOn,
                    CreatedBy = createdBy
                });
            }

            await AddRangeAsync(newCustomerBookingReportData);

            response.Code = ApiResponseCodes.OK;
            response.Payload = new List<CustomerBookingsReportViewModel>();
            response.Description = $"Successfully saved booking details with RefCodes ({string.Join(",", model.Select(m => m.RefCode))})";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            _logger.LogInformation(response.Description ?? string.Join(", ", response.Errors), model);
            return response;
        }

        public async Task<ApiResponse<PaginatedList<CustomerBookingsReportViewModel>>> GetCustomerBookings(GetCustomerBookingsReportViewModel model)
        {

            var response = new ApiResponse<PaginatedList<CustomerBookingsReportViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 1;
            model.PageTotal ??= 50;

            var validationResult = new GetCustomerBookingReportValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }

            if (!DateTime.TryParseExact(model.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid start date was specified. Date format should be 'd/M/yyyy";
                goto ReturnToCaller;
            }

            if (!DateTime.TryParseExact(model.EndDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid end date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            if (startDate > endDate)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Start date can not be greater than end date'";
                goto ReturnToCaller;
            }

            startDate = startDate.GetStartOfDay();
            endDate = endDate.GetEndOfDay();

            var data = await Task.Run(() =>
            {
                IQueryable<CustomerBookings> dataQuery;

                dataQuery = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate);

                if (!string.IsNullOrEmpty(model.BookingStatus))
                    dataQuery = dataQuery.Where(d => d.BookingStatus.Equals(model.BookingStatus));

                if (!string.IsNullOrEmpty(model.RefCode))
                    dataQuery = dataQuery.Where(d => d.RefCode.Equals(model.RefCode));

                return dataQuery.OrderBy(d => d.BookingDate).Select(d => (CustomerBookingsReportViewModel)d);
            });

            response.Payload = data.ToPaginatedList(model.PageIndex.Value, model.PageTotal.Value);
            response.TotalCount = response.Payload.TotalCount;
            response.PayloadMetaData = new PayloadMetaData(
                        pageIndex: response.Payload.PageIndex,
                        pageSize: response.Payload.PageSize,
                        totalPageCount: response.Payload.TotalPageCount,
                        totalCount: response.Payload.TotalCount);
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<CustomerBookingsReportViewModel>> UpdateCustomerBookings(UpdateCustomerBookingsReportViewModel model)
        {
            var response = new ApiResponse<CustomerBookingsReportViewModel>();

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new UpdateCustomerBookingReportValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();

                goto ReturnToCaller;
            }

            var existingData = UnitOfWork.Repository<CustomerBookings>()
                                        .GetFirstOrDefault(d => d.RefCode.Equals(model.RefCode));

            if (existingData == null)
            {
                response.Code = ApiResponseCodes.NOT_FOUND;
                response.Errors.Add($"No booking information with RefCode: {model.RefCode} was found");
                goto ReturnToCaller;
            }

            existingData.PaymentStatus = model.PaymentStatus;
            existingData.PaymentMethod = model.PaymentMethod;
            existingData.BookingStatus = model.BookingStatus;
            existingData.ModifiedOn = Clock.Now;
            existingData.ModifiedBy = Guid.Empty;

            var isCustomerBookingUpdated = await UpdateAsync(existingData) > 0;

            response.Code = isCustomerBookingUpdated ? ApiResponseCodes.OK : ApiResponseCodes.FAILED;
            response.Description = isCustomerBookingUpdated ? $"Booking information was successfully updated. RefCode {model.RefCode}" : $"Booking information was not successfully updated. RefCode {model.RefCode}";
            response.Payload = null;

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            _logger.LogInformation(response.Description ?? string.Join(", ", response.Errors), model);
            return response;
        }
    }
}
