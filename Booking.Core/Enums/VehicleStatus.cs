using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Booking.Core.Enums
{
    public enum VehicleStatus
    {
        [Description("In use")]
        IN_USE,
        [Description("In workshop")]
        IN_WORKSHOP,
        [Description("Sold")]
        Sold,
        [Description("Idle")]
        Idle
    }
}
