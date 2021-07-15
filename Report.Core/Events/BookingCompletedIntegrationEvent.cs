using Tornado.Shared.AzurePub.EventBus.Events;

namespace Report.Core
{
    public class BookingCompletedIntegrationEvent : IntegrationEvent
    {
        public string RefCode { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string SeatNo { get; set; }
        public string CustomerEmail { get; set; }
        public string DeparturePoint { get; set; }
        public string DestinationPoint { get; set; }
        public decimal Amount { get; set; }
        public string CustomerPhoneNumber { get; set; }

        public BookingCompletedIntegrationEvent(string refCode, string departureDate,
            string departureTime, string seatNo,
            string customerEmail, string departurePoint,
            string destinationPoint, decimal amount, string customerPhoneNumber)
        {
            RefCode = refCode;
            DepartureDate = departureDate;
            DepartureTime = departureTime;
            SeatNo = seatNo;
            CustomerEmail = customerEmail;
            DeparturePoint = departurePoint;
            DestinationPoint = destinationPoint;
            Amount = amount;
            CustomerPhoneNumber = customerPhoneNumber;
        }
    }
}