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
    public class BusboySalesReportService : Service<CustomerBookings>, IBusboySalesReportService
    {

        public BusboySalesReportService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<ApiResponse<PaginatedList<BusboySalesReportViewModel>>> GetBusboySalesReport(GetBusboySalesReportViewModel model)
        {
            var response = new ApiResponse<PaginatedList<BusboySalesReportViewModel>> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            model.PageIndex ??= 1;
            model.PageTotal ??= 50;

            var validationResult = new GetBusboySalesReportValidator().Validate(model);

            if (!validationResult.IsValid)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = validationResult.ToString(", ");
                goto ReturnToCaller;
            }

            DateTime startDate, endDate;

            if (!DateTime.TryParseExact(model.StartDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid start date was specified. Date format should be 'd/M/yyyy";
                goto ReturnToCaller;
            }

            if (DateTime.TryParseExact(model.EndDate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid end date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            startDate = startDate.GetStartOfDay();
            endDate = endDate.GetEndOfDay();

            var data = await Task.Run(() =>
            {
                return UnitOfWork.Repository<CustomerBookings>().GetAll()
                                 .Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate)
                                 .Where(d => d.BusboyId.Equals(model.BusboyId))
                                 .OrderBy(d => d.BookingDate)
                                 .Select(d => (BusboySalesReportViewModel)d);
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
