using System.ComponentModel;

namespace Booking.Core.Enums
{
    public enum PaymentStatus
    {
        PENDING,
        PAID,
        FAILED,
        REVERSED,
        [Description("Partial-Reversal")]
        PARTIAL_REVERSED,
        DECLINED
    }
}