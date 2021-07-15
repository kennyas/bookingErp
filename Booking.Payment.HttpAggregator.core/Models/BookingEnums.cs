namespace Booking.Payment.HttpAggregator.core.Models
{
    public enum PaymentStatus
    {
        PENDING,
        PAID,
        FAILED,
        REVERSED,
        PARTIAL_REVERSED,
        DECLINED
    }

    public enum BookingStatus
    {
        PENDING = 1,
        SCHEDULED,
        APPROVED,
        CANCELLED
    }
}