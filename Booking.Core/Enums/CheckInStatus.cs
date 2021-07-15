using System.ComponentModel;

namespace Booking.Core.Enums
{
    public enum CheckInStatus
    {
        PENDING,
        [Description("Checked-In")]
        CHECKED_IN,
        [Description("Checked-Out")]
        CHECKED_OUT
    }
}