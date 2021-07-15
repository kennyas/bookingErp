using Tornado.Shared.AzurePub.EventBus.Events;

namespace Report.Core
{
    public class BookingCreatedIntegrationEvent : IntegrationEvent
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
        public string CustomerName { get; set; }
        public string RouteId { get; set; }
        public string RouteName { get; set; }
        public string TripId { get; set; }
        public string TripName { get; set; }
        public string BusboyId { get; set; }
        public string BusboyUsername { get; set; }
        public string VehicleId { get; set; }
        public string VehicleChassisNumber { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public string DeparturePointId { get; set; }
        public string DestinationPointId { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }

        public BookingCreatedIntegrationEvent(
            string refCode,
            string departureDate,
            string departureTime,
            string seatNo,
            string customerEmail,
            string departurePoint,
            string destinationPoint,
            decimal amount,
            string customerPhoneNumber,
            string customerName,
            string routeId,
            string routeName,
            string tripId,
            string tripName,
            string busboyId,
            string busboyUsername,
            string vehicleId,
            string vehicleChassisNumber,
            string vehicleRegistrationNumber,
            string departurePointId,
            string destinationPointId,
            string paymentMethod,
            string paymentStatus
            )
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
            CustomerName = customerName;
            RouteId = routeId;
            RouteName = routeName;
            TripId = tripId;
            TripName = tripName;
            BusboyId = busboyId;
            BusboyUsername = busboyUsername;
            VehicleId = vehicleId;
            VehicleChassisNumber = vehicleChassisNumber;
            VehicleRegistrationNumber = vehicleRegistrationNumber;
            DeparturePointId = departurePointId;
            DestinationPointId = destinationPointId;
            PaymentMethod = paymentMethod;
            PaymentStatus = paymentStatus;
        }
    }
}