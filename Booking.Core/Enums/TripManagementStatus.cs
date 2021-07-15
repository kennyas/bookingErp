using System.ComponentModel;

namespace Booking.Core.Enums
{
    public enum TripManagementStatus
    {
        [Description("Pending")]
        PENDING=1,
        [Description("In-Transit")]
        IN_TRANSIT=2,
        [Description("Ended")]
        ENDED=3
    }
}