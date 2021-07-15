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
    public class TripSalesReportService : Service<CustomerBookings>, ITripSalesReportService
    {

        public TripSalesReportService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<ApiResponse<PaginatedList<TripSalesReportViewModel>>> GetTripSalesReport(GetTripSalesReportViewModel model)
        {
            var response = new ApiResponse<PaginatedList<TripSalesReportViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 1;
            model.PageTotal ??= 50;

            var validationResult = new GetTripSalesReportValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }

            Guid routeId = Guid.Empty;

            if (!string.IsNullOrEmpty(model.RouteId))
            {
                if (!Guid.TryParse(model.RouteId, out routeId))
                {
                    return new ApiResponse<PaginatedList<TripSalesReportViewModel>>(codes: ApiResponseCodes.INVALID_REQUEST, message: "Route id is invalid");
                }
            }


            DateTime startDate, endDate;

            if (!DateTime.TryParseExact(model.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid start date was specified. Date format should be 'd/M/yyyy";
                goto ReturnToCaller;
            }

            if (!DateTime.TryParseExact(model.EndDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid end date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            startDate = startDate.GetStartOfDay();
            endDate = endDate.GetEndOfDay();


            var paymentStatus_paid = Enum.GetName(typeof(Enums.PaymentStatus), Enums.PaymentStatus.PAID);

            var data = await Task.Run(() =>
            {
                var dataQuery = (UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking()
                      .Where(d => (d.BookingDate >= startDate && d.BookingDate <= endDate) && d.PaymentStatus.Equals(paymentStatus_paid)));

                if (!string.IsNullOrEmpty(model.RouteId))
                    dataQuery = dataQuery.Where(d => d.RouteId.Equals(routeId));

                return dataQuery.OrderBy(d => d.DepartureDate).Select(d => (TripSalesReportViewModel)d);
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
