using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Report.Core.Enums;
using Report.Core.Models;
using System;
using Tornado.Shared.Models;
using Tornado.Shared.ViewModels;

namespace Report.Core.ViewModels
{
    public class GetCustomerBookingsReportViewModel : BasePaginatedViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string RefCode { get; set; }
        public string BookingStatus { get; set; }
    }

    public class AddCustomerBookingsReportViewModel : BaseEntity
    {
        public string CustomerName { get; set; }
        public string BookingStatus { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public string DepartureTime { get; set; }
        public DateTime? DepartureDate { get; set; }

        public string PaymentStatus { get; set; }
        public string RefCode { get; set; }
        public Guid RouteId { get; set; }
        public string RouteName { get; set; }
        public Guid TripId { get; set; }
        public string TripName { get; set; }
        public Guid BusboyId { get; set; }
        public string BusboyUsername { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public Guid DepartureTerminalId { get; set; }
        public string DepartureTerminalName { get; set; }
        public Guid DestinationPickupPointId { get; set; }
        public string DestinationPickupPointName { get; set; }
        public string SeatNo { get; set; }
        public string VehicleChassisNumber { get; set; }
        public string DeparturePointId { get; set; }

        public static explicit operator AddCustomerBookingsReportViewModel(BookingCreatedIntegrationEvent source)
        {
            DateTime? depatureDate = null;

            if (!string.IsNullOrEmpty(source.DepartureDate))
                DateTime.Parse(source.DepartureDate);

            return new AddCustomerBookingsReportViewModel
            {
                BookingDate = source.CreationDate,
                RouteId = string.IsNullOrEmpty(source.RouteId) ? Guid.Empty : Guid.Parse(source.RouteId),
                RouteName = source.RouteName,
                BookingStatus = source.PaymentStatus,
                PaymentStatus = source.PaymentStatus,
                PaymentMethod = source.PaymentMethod,
                Amount = source.Amount,
                BusboyId = string.IsNullOrEmpty(source.BusboyId) ? Guid.Empty : Guid.Parse(source.BusboyId),
                BusboyUsername = source.BusboyUsername,
                CustomerName = source.CustomerName,
                DepartureTerminalId = string.IsNullOrEmpty(source.DeparturePointId) ? Guid.Empty : Guid.Parse(source.DeparturePointId),
                DepartureTerminalName = source.DeparturePoint,
                DestinationPickupPointId = string.IsNullOrEmpty(source.DeparturePointId) ? Guid.Empty : Guid.Parse(source.DeparturePointId),
                DestinationPickupPointName = source.DeparturePoint,
                DepartureTime = source.DepartureTime,
                DepartureDate = depatureDate,
                EmailAddress = source.CustomerEmail,
                PhoneNumber = source.CustomerPhoneNumber,
                RefCode = source.RefCode,
                TripId = string.IsNullOrEmpty(source.TripId) ? Guid.Empty : Guid.Parse(source.TripId),
                TripName = source.TripName,
                VehicleId = string.IsNullOrEmpty(source.VehicleId) ? Guid.Empty : Guid.Parse(source.VehicleId),
                VehicleChassisNumber = source.VehicleChassisNumber,
                VehicleRegistrationNumber = source.VehicleRegistrationNumber,
                DeparturePointId = source.DeparturePointId,
                SeatNo = source.SeatNo
            };
        }
    }

    public class UpdateCustomerBookingsReportViewModel : BaseEntity
    {
        public string CustomerName { get; set; }
        public string BookingStatus { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string RefCode { get; set; }
        public Guid RouteId { get; set; }
        public string DepartureTerminalName { get; set; }
        public string DestinationPickupPointName { get; set; }
        public string DepartureTime { get; set; }

        public static explicit operator UpdateCustomerBookingsReportViewModel(BookingCompletedIntegrationEvent source) => new UpdateCustomerBookingsReportViewModel
        {
            BookingDate = source.CreationDate,
            DepartureTerminalName = source.DeparturePoint,
            DestinationPickupPointName = source.DeparturePoint,
            DepartureTime = source.DepartureTime,
            EmailAddress = source.CustomerEmail,
            PhoneNumber = source.CustomerPhoneNumber,
            RefCode = source.RefCode
        };

        public static explicit operator UpdateCustomerBookingsReportViewModel(BookingCancelledIntegrationEvent source) => new UpdateCustomerBookingsReportViewModel
        {
            BookingDate = source.CreationDate,
            BookingStatus = Enum.GetName(typeof(BookingStatus), Enums.BookingStatus.CANCELLED),
            DepartureTerminalName = source.DeparturePoint,
            DestinationPickupPointName = source.DeparturePoint,
            DepartureTime = source.DepartureTime,
            EmailAddress = source.CustomerEmail,
            PhoneNumber = source.CustomerPhoneNumber,
            RefCode = source.RefCode
        };
    }


    public class CustomerBookingsReportViewModel
    {
        public string CustomerName { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public string PaymentMethod { get; set; }
        public string DeparturePickupPoint { get; set; }
        public string DestinationPickupPoint { get; set; }
        public string DepartureTime { get; set; }
        public string RouteName { get; set; }
        public Guid RouteId { get; set; }
        public string RefCode { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentStatus { get; set; }
        public Guid TripId { get; set; }
        public string TripName { get; set; }
        public Guid BusboyId { get; set; }
        public string BusboyUsername { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleRegistrationNumber { get; set; }
        public Guid DepartureTerminalId { get; set; }
        public string DepartureTerminalName { get; set; }
        public Guid DestinationPickupPointId { get; set; }
        public string DestinationPickupPointName { get; set; }


        public static explicit operator CustomerBookingsReportViewModel(CustomerBookings source) => new CustomerBookingsReportViewModel
        {
            CustomerName = source.CustomerName,
            BookingDate = source.BookingDate,
            BookingStatus = source.BookingStatus,
            DeparturePickupPoint = source.DepartureTerminalName,
            DepartureTime = source.DepatureTime,
            DestinationPickupPoint = source.DestinationPickupPointName,
            PaymentMethod = source.PaymentMethod,
            RouteId = source.RouteId,
            RouteName = source.RouteName,
            RefCode = source.RefCode,
            BusboyId = source.BusboyId ?? Guid.Empty,
            BusboyUsername = source.BusboyUsername,
            DepartureTerminalId = source.DepartureTerminalId,
            DepartureTerminalName = source.DepartureTerminalName,
            DestinationPickupPointId = source.DestinationPickupPointId,
            DestinationPickupPointName = source.DestinationPickupPointName,
            EmailAddress = source.EmailAddress,
            PaymentStatus = source.PaymentStatus,
            PhoneNumber = source.PhoneNumber,
            TripId = source.TripId,
            TripName = source.TripName,
            VehicleId = source.VehicleId,
            VehicleRegistrationNumber = source.VehicleRegistrationNumber
        };
    }

    public class GetBookedBusReportViewModel : BasePaginatedViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string VehicleId { get; set; }
        public string RouteId { get; set; }
        public string TripId { get; set; }
    }


    public class BookedBusReportViewModel
    {
        public string VehicleNumber { get; set; }
        public string RouteName { get; set; }
        public string TripName { get; set; }
        public Guid RouteId { get; set; }
        public Guid TripId { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string DepartureTime { get; set; }
        public Guid VehicleId { get; set; }
        public DateTime BookingDate { get; set; }

        public static explicit operator BookedBusReportViewModel(CustomerBookings source)
        {
            return new BookedBusReportViewModel
            {
                DepartureTime = source.DepatureTime,
                DepartureDate = source.DepartureDate,
                RouteId = source.RouteId,
                RouteName = source.RouteName,
                TripId = source.TripId,
                TripName = source.TripName,
                VehicleNumber = source.VehicleRegistrationNumber,
                BookingDate = source.BookingDate,
                VehicleId = source.VehicleId
            };
        }
    }

    public class GetBookedTicketReportViewModel : BasePaginatedViewModel
    {
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string ReferenceCode { get; set; }
    }
    public class AddBookedTicketReportViewModel : BaseEntity
    {
        public string ReferenceCode { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public decimal Amount { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Guid CustomerBookingReportId { get; set; }
    }

    public class UpdateBookedTicketReportViewModel : BaseEntity
    {
        public string ReferenceCode { get; set; }
        public string Status { get; set; }
        public DateTime CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }

    public class BookedTicketReportViewModel
    {
        public string ReferenceCode { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public string CustomerEmail { get; set; }
        public DateTime BookingDate { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }

        public static explicit operator BookedTicketReportViewModel(CustomerBookings source)
        {
            return new BookedTicketReportViewModel
            {
                CustomerEmail = source.EmailAddress,
                CustomerName = source.CustomerName,
                CustomerPhoneNumber = source.PhoneNumber,
                DateCreated = source.CreatedOn,
                DateModified = source.ModifiedOn,
                ReferenceCode = source.RefCode,
                Status = source.BookingStatus,
                BookingDate = source.BookingDate
            };
        }
    }

}
