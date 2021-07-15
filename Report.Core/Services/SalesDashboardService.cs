using Microsoft.EntityFrameworkCore;
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
    public class SalesDashboardService : Service<CustomerBookings>, ISalesDashboardService
    {
        public SalesDashboardService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<ApiResponse<TotalSalesViewModel>> GetTotalSales(GetTotalSalesViewModel model)
        {
            var response = new ApiResponse<TotalSalesViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            DateTime date;

            if (model.Date == null)
            {
                model.Date = Clock.Now.ToString("d/M/yyyy");
            }

            if (!DateTime.TryParseExact(model.Date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            DateTime startDate = date.GetStartOfDay();
            DateTime endDate = date.GetEndOfDay();

            var paymentStatus_paid = Enum.GetName(typeof(Enums.PaymentStatus), Enums.PaymentStatus.PAID);

            var data = await Task.Run(() =>
            {
                var dataQuery = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking()
                      .Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate)
                      .Where(d => d.PaymentStatus.Equals(paymentStatus_paid))
                      .Sum(d => d.Amount);

                return dataQuery;
            });

            response.Payload = new TotalSalesViewModel { Date = date.Date, Amount = data };
            response.TotalCount = response.Payload != null ? 1 : 0;
            response.Code = response.TotalCount == 0 ? ApiResponseCodes.NOT_FOUND : ApiResponseCodes.OK;
            response.Description = response.TotalCount > 0 ? "Successful" : "No record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<TotalSalesByRouteViewModel>>> GetTotalSalesByRoute(GetTotalSalesByRouteViewModel model)
        {
            var response = new ApiResponse<PaginatedList<TotalSalesByRouteViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetTotalSalesByRouteValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            DateTime startDate, endDate;

            if (model.Date == null)
            {
                model.Date = Clock.Now.ToString("d/M/yyyy");
            }

            if (!DateTime.TryParseExact(model.Date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            startDate = startDate.GetStartOfDay();
            endDate = startDate.GetEndOfDay();

            var paymentStatus_paid = Enum.GetName(typeof(Enums.PaymentStatus), Enums.PaymentStatus.PAID);

            var data = await Task.Run(() =>
            {
                var dataQuery = (UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().
                                Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate)).
                                Where(d => d.PaymentStatus.Equals(paymentStatus_paid));

                if (model.RouteId != null && model.RouteId?.Count != 0)
                    dataQuery = dataQuery.Where(d => model.RouteId.Contains(d.RouteId));

                return (dataQuery.OrderBy(d => d.BookingDate)
                .GroupBy(d => new { d.RouteId, d.RouteName }).
                                     Select(d => new TotalSalesByRouteViewModel
                                     {
                                         RouteId = d.Key.RouteId,
                                         TotalAmount = d.Sum(x => x.Amount),
                                         RouteName = d.Key.RouteName
                                     }));
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

        public async Task<ApiResponse<PaginatedList<TotalSalesByTerminalViewModel>>> GetTotalSalesByTerminal(GetTotalSalesByTerminalViewModel model)
        {
            var response = new ApiResponse<PaginatedList<TotalSalesByTerminalViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetTotalSalesByTerminalValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            DateTime startDate, endDate;

            if (model.Date == null)
            {
                model.Date = Clock.Now.ToString("d/M/yyyy");
            }

            if (!DateTime.TryParseExact(model.Date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            startDate = startDate.GetStartOfDay();
            endDate = startDate.GetEndOfDay();


            var paymentStatus_paid = Enum.GetName(typeof(Enums.PaymentStatus), Enums.PaymentStatus.PAID);


            var data = await Task.Run(() =>
            {

                var dataQuery = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().
                                                  Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate);

                if (model.DepartureTerminalId != null && model.DepartureTerminalId.Count > 0)
                    dataQuery = dataQuery.Where(d => model.DepartureTerminalId.Contains(d.DepartureTerminalId));


                return dataQuery.Where(d => d.PaymentStatus.Equals(paymentStatus_paid)).
                                                  OrderBy(d => d.BookingDate).
                                                  GroupBy(d => new { d.DepartureTerminalId, d.DepartureTerminalName }).
                                                  Select(d => new TotalSalesByTerminalViewModel
                                                  {
                                                      DepartureTerminalId = d.Key.DepartureTerminalId,
                                                      TotalAmount = d.Sum(d => d.Amount),
                                                      DepartureTerminalName = d.Key.DepartureTerminalName
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
            response.Description = response.TotalCount > 0 ? "Successful" : "No record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<TotalSalesByTripViewModel>>> GetTotalSalesByTrip(GetTotalSalesByTripViewModel model)
        {
            var response = new ApiResponse<PaginatedList<TotalSalesByTripViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetTotalSalesByTripValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }

            DateTime startDate, endDate;

            if (model.Date == null)
            {
                model.Date = Clock.Now.ToString("d/M/yyyy");
            }


            if (!DateTime.TryParseExact(model.Date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            startDate = startDate.GetStartOfDay();
            endDate = startDate.GetEndOfDay();


            var paymentStatus_paid = Enum.GetName(typeof(Enums.PaymentStatus), Enums.PaymentStatus.PAID);

            var data = await Task.Run(() =>
            {

                var dataQuery = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().
                                Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate);

                if (model.TripId != null && model.TripId?.Count > 0)
                    dataQuery = dataQuery.Where(d => model.TripId.Contains(d.TripId));


                return dataQuery.Where(d => d.PaymentStatus.Equals(paymentStatus_paid)).
                                OrderBy(d => d.BookingDate).
                                GroupBy(d => new { d.TripId, d.TripName, d.BookingDate, d.DepartureDate }).
                                Select(d => new TotalSalesByTripViewModel
                                {
                                    TripId = d.Key.TripId,
                                    TotalAmount = d.Sum(d => d.Amount),
                                    TripName = d.Key.TripName,
                                    BookingDate = d.Key.BookingDate,
                                    DepatureDate = d.Key.DepartureDate
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
            response.Description = response.TotalCount > 0 ? "Successful" : "No record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }

        public async Task<ApiResponse<PaginatedList<TotalSalesByVehicleViewModel>>> GetTotalSalesByVehicle(GetTotalSalesByVehicleViewModel model)
        {
            var response = new ApiResponse<PaginatedList<TotalSalesByVehicleViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            var validationResult = new GetTotalSalesByVehicleValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors = validationResult.Errors.Select(error => error.ErrorMessage).ToList();
                goto ReturnToCaller;
            }


            DateTime startDate, endDate;

            if (model.Date == null)
            {
                model.Date = Clock.Now.ToString("d/M/yyyy");
            }

            if (!DateTime.TryParseExact(model.Date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            var paymentStatus_paid = Enum.GetName(typeof(Enums.PaymentStatus), Enums.PaymentStatus.PAID);

                startDate = startDate.GetStartOfDay();
                endDate = startDate.GetEndOfDay();

                var data = await Task.Run(() =>
                {

                    var dataQuery = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().
                                                     Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate);

                    if (model.VehicleId != null && model.VehicleId?.Count > 0)
                        dataQuery = dataQuery.Where(d => model.VehicleId.Contains(d.VehicleId));


                    return dataQuery.Where(d => d.PaymentStatus.Equals(paymentStatus_paid)).
                                                   OrderBy(d => d.BookingDate).
                                                   GroupBy(d => new
                                                   {
                                                       d.VehicleId,
                                                       d.VehicleRegistrationNumber
                                                   }).
                                                   Select(d => new TotalSalesByVehicleViewModel
                                                   {
                                                       VehicleId = d.Key.VehicleId,
                                                       TotalAmount = d.Sum(d => d.Amount),
                                                       RegistrationNumber = d.Key.VehicleRegistrationNumber,
                                                       Date = startDate.Date
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
            response.Description = response.TotalCount > 0 ? "Successful" : "No record found";

        ReturnToCaller:
            response.ResponseCode = ResponseCodeHelper.OK.ToString();
            return response;
        }
    }
}
