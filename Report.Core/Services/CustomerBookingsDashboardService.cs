using Microsoft.EntityFrameworkCore;
using Report.Core.Enums;
using Report.Core.Models;
using Report.Core.Services.Interfaces;
using Report.Core.Validators;
using Report.Core.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Enums;
using Tornado.Shared.Utils;
using Tornado.Shared.ViewModels;

namespace Report.Core.Services
{
    public class CustomerBookingsDashboardService : Service<CustomerBookings>, ICustomerBookingsDashboardService
    {

        public CustomerBookingsDashboardService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }

        public async Task<ApiResponse<CustomerBookingsDashboardWithDataViewModel>> GetCustomerBookingsDashboardByDate(GetCustomerBookingsDashboardByDateViewModel model)
        {

            var response = new ApiResponse<CustomerBookingsDashboardWithDataViewModel> { Code = ApiResponseCodes.OK };

            if (model == null)
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Errors.Add("Model can not be empty");
                goto ReturnToCaller;
            }

            DateTime endDate;

            if (!DateTime.TryParseExact(model.Date, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
            {
                response.Code = ApiResponseCodes.INVALID_REQUEST;
                response.Description = "Invalid date was specified. Date format should be 'd/M/yyyy'";
                goto ReturnToCaller;
            }

            endDate = startDate.GetEndOfDay();

            var data = await Task.Run(() => (UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().Where(d => d.BookingDate >= startDate &&  d.BookingDate <= endDate).Select(d => d).ToList()));

            string bookingStatusApproved = Enum.GetName(typeof(BookingStatus), BookingStatus.APPROVED)?.ToLower();
            string bookingStatusPending = Enum.GetName(typeof(BookingStatus), BookingStatus.PENDING)?.ToLower();
            string bookingStatusCancelled = Enum.GetName(typeof(BookingStatus), BookingStatus.CANCELLED)?.ToLower();
            string bookingStatusScheduled = Enum.GetName(typeof(BookingStatus), BookingStatus.SCHEDULED)?.ToLower();

            if (data.Count == 0)
                goto ReturnToCaller;

            DateTime currentDate = DateTime.Now.GetStartOfDay();
            var cancelledBookings = (data.Where(d => d.BookingStatus.Equals(bookingStatusCancelled, StringComparison.InvariantCultureIgnoreCase)).Select(d => new CustomerBookingsDashboardDataViewModel
            {
                BookingDate = d.BookingDate,
                BookingStatus = d.BookingStatus,
                CustomerName = d.CustomerName,
                PaymentMethod = d.PaymentMethod,
                PaymentStatus = d.PaymentStatus,
                RefCode = d.RefCode
            })).ToList();

            var successfulBookings = (data.Where(d =>
                              d.BookingStatus.Equals(bookingStatusApproved, StringComparison.InvariantCultureIgnoreCase)
                              ).Select(d => new CustomerBookingsDashboardDataViewModel
                              {
                                  BookingDate = d.BookingDate,
                                  BookingStatus = d.BookingStatus,
                                  CustomerName = d.CustomerName,
                                  PaymentMethod = d.PaymentMethod,
                                  PaymentStatus = d.PaymentStatus,
                                  RefCode = d.RefCode
                              })).ToList();

            var pendingBookings = (data.Where(d =>
                             d.BookingStatus.Equals(bookingStatusPending, StringComparison.InvariantCultureIgnoreCase) ||
                             d.BookingStatus.Equals(bookingStatusScheduled, StringComparison.InvariantCultureIgnoreCase)
                                ).Select(d => new CustomerBookingsDashboardDataViewModel
                                {
                                    BookingDate = d.BookingDate,
                                    BookingStatus = d.BookingStatus,
                                    CustomerName = d.CustomerName,
                                    PaymentMethod = d.PaymentMethod,
                                    PaymentStatus = d.PaymentStatus,
                                    RefCode = d.RefCode
                                })).ToList();

            var unsuccessfulBookings = data.Where(d =>
                !d.BookingStatus.Equals(bookingStatusApproved, StringComparison.InvariantCultureIgnoreCase) &&
                !d.BookingStatus.Equals(bookingStatusPending, StringComparison.InvariantCultureIgnoreCase) &&
                !d.BookingStatus.Equals(bookingStatusScheduled, StringComparison.InvariantCultureIgnoreCase))
                .Select(d => new CustomerBookingsDashboardDataViewModel
                {
                    BookingDate = d.BookingDate,
                    BookingStatus = d.BookingStatus,
                    CustomerName = d.CustomerName,
                    PaymentMethod = d.PaymentMethod,
                    PaymentStatus = d.PaymentStatus,
                    RefCode = d.RefCode
                }).ToList();

            response.Payload = new CustomerBookingsDashboardWithDataViewModel
            {
                TotalBookings = data.Count,
                TotalCancelledBookings = cancelledBookings.Count,
                TotalNumberOfSuccessfulBookings = successfulBookings.Count,
                TotalNumberOfUnsuccessfulBookings = unsuccessfulBookings.Count,
                TotalNumberOfPendingBookings = pendingBookings.Count,
                BookingDate = currentDate,
                Data = new CustomerBookingsDashboardMetaDataViewModel
                {
                    CancelledBookings = cancelledBookings,
                    SuccessfulBookings = successfulBookings,
                    UnsuccessfulBookings = unsuccessfulBookings,
                    PendingBookings = pendingBookings
                }
            };

        ReturnToCaller:
            response.Code = ApiResponseCodes.OK;
            response.Description = $"Successful";
            return response;
        }
    }
}
