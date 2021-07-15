using Report.Core.Models;
using System;
using System.Collections.Generic;
using Tornado.Shared.ViewModels;

namespace Report.Core.ViewModels
{
    public class GetCustomerBookingsDashboardByDateRangeViewModel : BasePaginatedViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class GetCustomerBookingsDashboardByDateViewModel : BasePaginatedViewModel
    {
        public string Date { get; set; }
    }

    public class CustomerBookingsDashboardWithDataViewModel
    {
        public int TotalBookings { get; set; }
        public int TotalCancelledBookings { get; set; }
        public int TotalNumberOfSuccessfulBookings { get; set; }
        public int TotalNumberOfPendingBookings { get; set; }
        public int TotalNumberOfUnsuccessfulBookings { get; set; }
        public DateTime BookingDate { get; set; }
        public CustomerBookingsDashboardMetaDataViewModel Data { get; set; }
    }

    public class CustomerBookingsDashboardMetaDataViewModel
    {
        public List<CustomerBookingsDashboardDataViewModel> CancelledBookings { get; set; }
        public List<CustomerBookingsDashboardDataViewModel> SuccessfulBookings { get; set; }
        public List<CustomerBookingsDashboardDataViewModel> UnsuccessfulBookings { get; set; }
        public List<CustomerBookingsDashboardDataViewModel> PendingBookings { get; set; }
    }

    public class CustomerBookingsDashboardDataViewModel
    {
        public string RefCode { get; set; }
        public string CustomerName { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
    }
}
