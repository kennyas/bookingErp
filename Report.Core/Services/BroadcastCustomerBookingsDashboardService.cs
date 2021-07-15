using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Report.Core.Enums;
using Report.Core.Hubs;
using Report.Core.Hubs.Interfaces;
using Report.Core.Models;
using Report.Core.Services.Interfaces;
using Report.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Utils;

namespace Report.Core.Services
{
    public class BroadcastCustomerBookingsDashboardService : Service<CustomerBookings>, IBroadcastCustomerBookingsDashboardService
    {
        private readonly IHubContext<CustomerBookingsDashboardHub, ICustomerBookingsDashboardHub> _customerBookingsDashboardHub;

        public BroadcastCustomerBookingsDashboardService(IUnitOfWork unitOfWork, IHubContext<CustomerBookingsDashboardHub, ICustomerBookingsDashboardHub> customerBookingsDashboardHub) : base(unitOfWork)
        {
            _customerBookingsDashboardHub = customerBookingsDashboardHub;
        }

        public List<CustomerBookings> Data { get; set; }

        public async Task BroadcastAll()
        {
            await Task.Run(() =>
           {
               _ = CurrentCustomerBookingsDashboard();
           });
        }

        public CustomerBookingsDashboardWithDataViewModel GetCurrentCustomerBookingsDashboard(List<CustomerBookings> data = null)
        {
            if (data != null)
                Data = data;

             if (Data == null)
                LoadData();

            CustomerBookingsDashboardWithDataViewModel currentCustomerBookingsDashboard = new CustomerBookingsDashboardWithDataViewModel();

            string bookingStatusApproved = Enum.GetName(typeof(BookingStatus), BookingStatus.APPROVED)?.ToLower();
            string bookingStatusPending = Enum.GetName(typeof(BookingStatus), BookingStatus.PENDING)?.ToLower();
            string bookingStatusCancelled = Enum.GetName(typeof(BookingStatus), BookingStatus.CANCELLED)?.ToLower();

            if (Data.Count == 0)
                goto ReturnToCaller;

            DateTime currentDate = DateTime.Now.GetStartOfDay();
            var cancelledBookings = (Data.Where(d => d.BookingStatus.Equals(bookingStatusCancelled, StringComparison.InvariantCultureIgnoreCase)).Select(d => new CustomerBookingsDashboardDataViewModel
            {
                BookingDate = d.BookingDate,
                BookingStatus = d.BookingStatus,
                CustomerName = d.CustomerName,
                PaymentMethod = d.PaymentMethod,
                PaymentStatus = d.PaymentStatus,
                RefCode = d.RefCode
            })).ToList();

            var successfulBookings = (Data.Where(d =>
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

            var pendingBookings = (Data.Where(d =>
                             d.BookingStatus.Equals(bookingStatusPending, StringComparison.InvariantCultureIgnoreCase)
                                ).Select(d => new CustomerBookingsDashboardDataViewModel
                                {
                                    BookingDate = d.BookingDate,
                                    BookingStatus = d.BookingStatus,
                                    CustomerName = d.CustomerName,
                                    PaymentMethod = d.PaymentMethod,
                                    PaymentStatus = d.PaymentStatus,
                                    RefCode = d.RefCode
                                })).ToList();

            var unsuccessfulBookings = Data.Where(d =>
                !d.BookingStatus.Equals(bookingStatusApproved, StringComparison.InvariantCultureIgnoreCase) &&
                !d.BookingStatus.Equals(bookingStatusPending, StringComparison.InvariantCultureIgnoreCase))
                .Select(d => new CustomerBookingsDashboardDataViewModel
                {
                    BookingDate = d.BookingDate,
                    BookingStatus = d.BookingStatus,
                    CustomerName = d.CustomerName,
                    PaymentMethod = d.PaymentMethod,
                    PaymentStatus = d.PaymentStatus,
                    RefCode = d.RefCode
                }).ToList();

            currentCustomerBookingsDashboard = new CustomerBookingsDashboardWithDataViewModel
            {
                TotalBookings = Data.Count,
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
            return currentCustomerBookingsDashboard;
        }

        public async Task CurrentCustomerBookingsDashboard()
        {
            await _customerBookingsDashboardHub.Clients.All.CurrentCustomerBookingsDashboardWithData(GetCurrentCustomerBookingsDashboard());
        }

        public BroadcastCustomerBookingsDashboardService LoadData()
        {
            DateTime startDate = DateTime.Now.GetStartOfDay();
            DateTime endDate = startDate.GetEndOfDay();

            Data = UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().Where(d => d.BookingDate >= startDate &&  d.BookingDate <= endDate).Select(d => d).ToList();

            return this;
        }

        public BroadcastCustomerBookingsDashboardService LoadData(List<CustomerBookings> dataSource)
        {
            Data = dataSource;
            return this;
        }

        public List<CustomerBookings> GetData()
        {
            DateTime startDate = DateTime.Now.GetStartOfDay();
            DateTime endDate = startDate.GetEndOfDay();

            return UnitOfWork.Repository<CustomerBookings>().GetAll().AsNoTracking().Where(d => d.BookingDate >= startDate && d.BookingDate <= endDate ).Select(d => d).ToList();
        }
    }
}
