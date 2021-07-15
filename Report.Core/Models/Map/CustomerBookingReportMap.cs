using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Report.Core.Enums;
using System;
using Tornado.Shared.Models.Map;
using Tornado.Shared.Statics;
using Tornado.Shared.Timing;

namespace Report.Core.Models.Map
{

    public class CustomerBookingReportMap : BaseEntityTypeConfiguration<CustomerBookings>
    {
        public override void Configure(EntityTypeBuilder<CustomerBookings> builder)
        {
            builder.HasData(
              new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-3),
                  CreatedOn = Clock.Now.AddDays(-3),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.APPROVED),
                  BusboyId = Defaults.SysUserId,
                  BusboyUsername = "Maduka",
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-100",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              }, new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-2),
                  CreatedOn = Clock.Now.AddDays(-2),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.APPROVED),
                  BusboyId = Defaults.SysUserId,
                  BusboyUsername = "Maduka",
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-101",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              }, new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-1),
                  CreatedOn = Clock.Now.AddDays(-1),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.APPROVED),
                  BusboyId = Defaults.SysUserId,
                  BusboyUsername = "Maduka",
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-102",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              },

              new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-3),
                  CreatedOn = Clock.Now.AddDays(-3),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.PENDING),
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PENDING),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-200",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              },

              new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-2),
                  CreatedOn = Clock.Now.AddDays(-2),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.PENDING),
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PENDING),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-202",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              },

              new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-1),
                  CreatedOn = Clock.Now.AddDays(-1),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.PENDING),
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PENDING),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-202",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              },

              new CustomerBookings
              {
                  BookingDate = Clock.Now.AddDays(-3),
                  CreatedOn = Clock.Now.AddDays(-3),
                  BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.CANCELLED),
                  CustomerName = Defaults.CorporateUserMobile,
                  DepartureTerminalId = PickUpPointDefaults.PPId4,
                  DepartureTerminalName = PickUpPointDefaults.PPName4,
                  EmailAddress = Defaults.CustomerUserEmail,
                  DestinationPickupPointId = PickUpPointDefaults.PPId1,
                  DepatureTime = "2:00 pm",
                  DestinationPickupPointName = PickUpPointDefaults.PPName1,
                  PaymentMethod = "Card payment",
                  PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.FAILED),
                  PhoneNumber = Defaults.CustomerUserMobile,
                  RefCode = "xxxx-xxxx",
                  RouteId = RouteDefaults.RouteId1,
                  RouteName = RouteDefaults.RouteName1,
                  TripId = Guid.NewGuid(),
                  TripName = "DEMO-trip-300",
                  VehicleId = VehicleDefaults.VehicleId1,
                  VehicleRegistrationNumber = "XXXXXXX",
                  Amount = 3000
              },

                   new CustomerBookings
                   {
                       BookingDate = Clock.Now.AddDays(-2),
                       CreatedOn = Clock.Now.AddDays(-2),
                       BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.CANCELLED),
                       CustomerName = Defaults.CorporateUserMobile,
                       DepartureTerminalId = PickUpPointDefaults.PPId4,
                       DepartureTerminalName = PickUpPointDefaults.PPName4,
                       EmailAddress = Defaults.CustomerUserEmail,
                       DestinationPickupPointId = PickUpPointDefaults.PPId1,
                       DepatureTime = "2:00 pm",
                       DestinationPickupPointName = PickUpPointDefaults.PPName1,
                       PaymentMethod = "Card payment",
                       PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.FAILED),
                       PhoneNumber = Defaults.CustomerUserMobile,
                       RefCode = "xxxx-xxxx",
                       RouteId = RouteDefaults.RouteId1,
                       RouteName = RouteDefaults.RouteName1,
                       TripId = Guid.NewGuid(),
                       TripName = "DEMO-trip-301",
                       VehicleId = VehicleDefaults.VehicleId1,
                       VehicleRegistrationNumber = "XXXXXXX",
                       Amount = 3000
                   },

                   new CustomerBookings
                   {
                       BookingDate = Clock.Now.AddDays(-1),
                       CreatedOn = Clock.Now.AddDays(-1),
                       BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.CANCELLED),
                       CustomerName = Defaults.CorporateUserMobile,
                       DepartureTerminalId = PickUpPointDefaults.PPId4,
                       DepartureTerminalName = PickUpPointDefaults.PPName4,
                       EmailAddress = Defaults.CustomerUserEmail,
                       DestinationPickupPointId = PickUpPointDefaults.PPId1,
                       DepatureTime = "2:00 pm",
                       DestinationPickupPointName = PickUpPointDefaults.PPName1,
                       PaymentMethod = "Card payment",
                       PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.FAILED),
                       PhoneNumber = Defaults.CustomerUserMobile,
                       RefCode = "xxxx-xxxx",
                       RouteId = RouteDefaults.RouteId1,
                       RouteName = RouteDefaults.RouteName1,
                       TripId = Guid.NewGuid(),
                       TripName = "DEMO-trip-302",
                       VehicleId = VehicleDefaults.VehicleId1,
                       VehicleRegistrationNumber = "XXXXXXX",
                       Amount = 3000
                   },

                   new CustomerBookings
                   {
                       BookingDate = Clock.Now.AddDays(-3),
                       CreatedOn = Clock.Now.AddDays(-3),
                       BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.SCHEDULED),
                       CustomerName = Defaults.CorporateUserMobile,
                       DepartureTerminalId = PickUpPointDefaults.PPId4,
                       DepartureTerminalName = PickUpPointDefaults.PPName4,
                       EmailAddress = Defaults.CustomerUserEmail,
                       DestinationPickupPointId = PickUpPointDefaults.PPId1,
                       DepatureTime = "2:00 pm",
                       DestinationPickupPointName = PickUpPointDefaults.PPName1,
                       PaymentMethod = "Card payment",
                       PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID),
                       PhoneNumber = Defaults.CustomerUserMobile,
                       RefCode = "xxxx-xxxx",
                       RouteId = RouteDefaults.RouteId1,
                       RouteName = RouteDefaults.RouteName1,
                       TripId = Guid.NewGuid(),
                       TripName = "DEMO-trip-400",
                       VehicleId = VehicleDefaults.VehicleId1,
                       VehicleRegistrationNumber = "XXXXXXX",
                       Amount = 3000
                   },
                   new CustomerBookings
                   {
                       BookingDate = Clock.Now.AddDays(-2),
                       CreatedOn = Clock.Now.AddDays(-2),
                       BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.SCHEDULED),
                       CustomerName = Defaults.CorporateUserMobile,
                       DepartureTerminalId = PickUpPointDefaults.PPId4,
                       DepartureTerminalName = PickUpPointDefaults.PPName4,
                       EmailAddress = Defaults.CustomerUserEmail,
                       DestinationPickupPointId = PickUpPointDefaults.PPId1,
                       DepatureTime = "2:00 pm",
                       DestinationPickupPointName = PickUpPointDefaults.PPName1,
                       PaymentMethod = "Card payment",
                       PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID),
                       PhoneNumber = Defaults.CustomerUserMobile,
                       RefCode = "xxxx-xxxx",
                       RouteId = RouteDefaults.RouteId1,
                       RouteName = RouteDefaults.RouteName1,
                       TripId = Guid.NewGuid(),
                       TripName = "DEMO-trip-401",
                       VehicleId = VehicleDefaults.VehicleId1,
                       VehicleRegistrationNumber = "XXXXXXX",
                       Amount = 3000
                   },
                   new CustomerBookings
                   {
                       BookingDate = Clock.Now.AddDays(-1),
                       CreatedOn = Clock.Now.AddDays(-1),
                       BookingStatus = Enum.GetName(typeof(BookingStatus), BookingStatus.SCHEDULED),
                       CustomerName = Defaults.CorporateUserMobile,
                       DepartureTerminalId = PickUpPointDefaults.PPId4,
                       DepartureTerminalName = PickUpPointDefaults.PPName4,
                       EmailAddress = Defaults.CustomerUserEmail,
                       DestinationPickupPointId = PickUpPointDefaults.PPId1,
                       DepatureTime = "2:00 pm",
                       DestinationPickupPointName = PickUpPointDefaults.PPName1,
                       PaymentMethod = "Card payment",
                       PaymentStatus = Enum.GetName(typeof(PaymentStatus), PaymentStatus.PAID),
                       PhoneNumber = Defaults.CustomerUserMobile,
                       RefCode = "xxxx-xxxx",
                       RouteId = RouteDefaults.RouteId1,
                       RouteName = RouteDefaults.RouteName1,
                       TripId = Guid.NewGuid(),
                       TripName = "DEMO-trip-402",
                       VehicleId = VehicleDefaults.VehicleId1,
                       VehicleRegistrationNumber = "XXXXXXX",
                       Amount = 3000
                   }
              );
            base.Configure(builder);
        }
    }
}
