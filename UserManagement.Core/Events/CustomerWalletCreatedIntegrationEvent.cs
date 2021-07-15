using Tornado.Shared.AzurePub.EventBus.Events;

namespace UserManagement.Core.Events
{
    public class CustomerWalletCreatedIntegrationEvent : IntegrationEvent
    {
        public string PhoneNumber { get; set; }
        public string CustomerId { get; set; }
        public CustomerWalletCreatedIntegrationEvent(string phoneNumber, string customerId)
        {
            PhoneNumber = phoneNumber;
            CustomerId = customerId;
        }
    }
}
