namespace Booking.Core.Helpers
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";


        public class GigLogisticsUrlConfig
        {
            public string Base { get; set; }
            public static string GetWayBill(string refcode) => $"/api/webtracking/getwaybill/{refcode}";
        }
        
        public class GigMobilityUrlConfig
        {
            public string Base { get; set; }

            public static string GetRefCodeDetails(string refCode) => $"/api/bookings/details/{refCode}";

            //public const string DebitWallet = "/api/walletapi/debitcustomerwallet";
        }

        public class Keys
        {
            public const string Client = "http_client";
            public const string WalletBaseUrl = "walleServiceUrl";
        }

        public class TransactionType
        {
            public const string WalletDebit = "Wallet Debit";
            public const string WalletTransfer = "Wallet Transfer";
        }

        public class GIGBookingChannel
        {
            public const string GIGL = "gigl";
            public const string GIGM = "gigm";
            //public static string Unknown { get; set; }
        }
    }
}