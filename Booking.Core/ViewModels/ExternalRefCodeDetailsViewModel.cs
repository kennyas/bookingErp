using Booking.Core.Enums;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class ExternalGIGLRefCodeDetailsViewModel
    {
        public CustomerDetails Object { get; set; }

        public HttpStatusCode Code { get; set; }

        public string ShortDescription { get; set; }

        public string magayaErrorMessage { get; set; }

        public ValidationErrors ValidationErrors { get; set; }
    }

    public class ValidationErrors
    {
        public List<string> Errors { get; set; }
    }
    public class GIGLResponseCodes
    {
        public const string Success = "200";
    }

    public class CustomerDetails
    {
        public string Waybill { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime DateCreated { get; set; }
        public string CustomerNumber { get; set; }
    }
}
