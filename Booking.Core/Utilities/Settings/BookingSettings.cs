using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.Utilities.Settings
{
    public class BookingSettings  
    {
        public string PicBaseUrl { get; set; }

        public string EventBusConnection { get; set; }

        public bool UseCustomizationData { get; set; }
        public bool AzureStorageEnabled { get; set; }
    }
}
