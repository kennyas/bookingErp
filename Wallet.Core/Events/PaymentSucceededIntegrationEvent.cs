using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Wallet.Core.Events
{
    public class PaymentSucceededIntegrationEvent : IntegrationEvent
    {
        public Guid CustomerId { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerName { get; set; }
        public string TransType { get; set; } 
        public string PaymentRefCode { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public decimal Amount { get; set; }
        public decimal AvailBalance { get; set; }
        public string PaymentDate { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public PaymentSucceededIntegrationEvent(
            Guid customerId, string paymentRefCode,
            string responseCode, string responseMessage, decimal amount, decimal availBalance,
            string paymentDate, string customerEmail, string transType, string customerName,
            string customerPhoneNumber
            )
        {
            CustomerId = customerId;
            PaymentRefCode = paymentRefCode;
            Amount = amount;
            AvailBalance = availBalance;
            PaymentDate = paymentDate;
            ResponseCode = responseCode;
            ResponseMessage = responseMessage;
            CustomerEmail = customerEmail;
            CustomerName = customerName;
            TransType = transType;
            CustomerPhoneNumber = customerPhoneNumber;
        }
    }
}
