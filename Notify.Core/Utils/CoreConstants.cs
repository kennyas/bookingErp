namespace Notify.Core
{
    public abstract class CoreConstants
    {
        public const string DateFormat = "dd MMMM, yyyy";
        public const string TimeFormat = "hh:mm tt";
        public const string SystemDateFormat = "dd/MM/yyyy";
        public const string CurrencyFormat = "N2"; 

        public class Url
        {
            public const string PasswordResetEmail = "messaging/emailtemplates/password-reset-email.html";
            public const string AccountActivationEmail = "messaging/emailtemplates/account-email.html";
            public const string ActivationCodeEmail = "messaging/emailtemplates/activation-code-email.html";
            public const string NewAccountEmail = "messaging/emailtemplates/new-account-email.html";
            public const string BookingConfirmation = "messaging/emailtemplates/booking-confirmation.html";
            public const string WalletTransactNotify = "messaging/emailtemplates/ wallet-transact-notify";
        }
    }
}