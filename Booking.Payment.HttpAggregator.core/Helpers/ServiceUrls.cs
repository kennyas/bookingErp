namespace Booking.Payment.HttpAggregator.core.Helpers
{
    public class BookingServiceUrlsConfig
    {
        public string Base { get; set; }
        public static string GetByRefCode(string refcode) => $"/api/bookingapi/getentitybooking?refcode={refcode}";
        public static string VerifyGIGLExternalBookingRefCode => $"/api/bookingapi/verifyandpayviarefcode";

    }

    public class WalletServiceUrlsConfig
    {
        public string Base { get; set; }
        public static string Debit() => "/api/walletapi/debitcustomerwallet";
    }
}