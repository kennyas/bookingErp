using Tornado.Shared.AzurePub.EventBus.Events;

namespace Booking.Core
{
    public class BookingCancelledIntegrationEvent : IntegrationEvent
    {
        public string RefCode { get; set; }
        public string DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public string SeatNo { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string DeparturePoint { get; set; }
        public string DestinationPoint { get; set; }
        public string Route { get; set; }
        public string BookingDate { get; set; }

        public BookingCancelledIntegrationEvent(string refCode, string departureDate,
            string departureTime, string seatNo,
            string customerEmail, string departurePoint,
            string destinationPoint,string route, 
            string customerPhoneNumber, string bookingDate)
        {
            RefCode = refCode;
            DepartureDate = departureDate;
            DepartureTime = departureTime;
            SeatNo = seatNo;
            CustomerEmail = customerEmail;
            DeparturePoint = departurePoint;
            DestinationPoint = destinationPoint;
            BookingDate = bookingDate;
            Route = route;
            CustomerPhoneNumber = customerPhoneNumber;
        }
    }
}