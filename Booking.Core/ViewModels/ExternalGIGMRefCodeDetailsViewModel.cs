using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class ExternalGIGMRefCodeDetailsViewModel
    {

        public HttpStatusCode Code { get; set; }

        public string ShortDescription { get; set; }
        public GIGMCustomerDetails Object { get; set; }

        public ValidationErrors ValidationErrors { get; set; }

    }

    public class GIGMCustomerDetails
    {
        public string BookingReferenceCode { get; set; }
        public GIGBookingStatus BookingStatus { get; set; }
        public DateTime? DepartureDate { get; set; }
        public DateTime DateCreated { get; set; }
        public string refCode { get; set; }
        public string PhoneNumber { get; set; }
    }

    public enum GIGBookingStatus
    {
        Pending,
        Approved,
        Cancelled,
        Created,
        Declined,
        Expired,
        Failed,
        OnLock,
        OnPayment,
        Ongoing,
        Abandoned,
        Refunded,
        Reversed,
        TransactionError,
        Unsuccessful,
        GtbCancelled,
        Suspended
    }
}
