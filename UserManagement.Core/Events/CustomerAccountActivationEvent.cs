using Tornado.Shared.AzurePub.EventBus.Events;

namespace UserManagement.Core.Events
{
     //follow convention... add the INtegrationEvent at the end
    public class CustomerAccountActivationIntegrationEvent : IntegrationEvent
    {
        public string OTP { get; }
        public string PhoneNumber { get; }
        public string UserName { get; }
        public string Email { get; }
        public string DialingCode { get; set; }

        public CustomerAccountActivationIntegrationEvent(string userName, string email,string dialingCode, string phoneNumber, string otp)
        {
            Email = email;
            OTP = otp;
            UserName = userName;
            PhoneNumber = phoneNumber;
            DialingCode = dialingCode;
        }
    }
}