namespace Booking.Core.Enums
{
    public enum BookingStatus
    {
        PENDING = 1,
        //PROCESSING,
        SCHEDULED,

        //Customers should only be able to pay for these ones
        APPROVED,

        //Customers should only be able to proceed to book these two
        CANCELLED,
    }
}