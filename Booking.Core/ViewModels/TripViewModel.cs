using Booking.Core.Dtos;
using Booking.Core.Models;
using System;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class GetAllTripViewModel : BasePaginatedViewModel
    {
    }

    public class GetTripByIdViewModel
    {
        public string Id { get; set; }
    }

    public class GetTripsByDateRangeViewModel : BasePaginatedViewModel
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    public class GetTripsWithActiveDiscountViewModel : BasePaginatedViewModel
    {
    }


    public class GetTripsByVehicleIdViewModel : BasePaginatedViewModel
    {
        public Guid VehicleId { get; set; }
    }
    public class GetTripsByVehicleRegNoViewModel : BasePaginatedViewModel
    {
        public string VehicleRegistrationNumber { get; set; }
    }
    public class ViewTripViewModel : BaseViewModel
    {
        public string DepartureTime { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DispatchFee { get; set; }
        public string Title { get; set; }

        public decimal? Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        public bool CanBeScheduled { get; set; }
        public int? NoOfTrips { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleModelTitle { get; set; }

        public TripDaysViewModel TripDaysViewModel { get; set; } = new TripDaysViewModel();

        public static explicit operator ViewTripViewModel(TripDto source)
        {
            var destination = new ViewTripViewModel
            {
                DepartureTime = source.DepartureTime,
                Discount = source.Discount,
                VehicleId = source.VehicleId,
               // BaseFee = source.BaseFee,
                CanBeScheduled = source.CanBeScheduled,
                TotalCount = source.TotalCount,
                DiscountEndDate = source.DiscountEndDate,
                DiscountStartDate = source.DiscountStartDate,
                Id = source.Id.ToString(),
                NoOfTrips = source.NoOfTrips,
                Title = source.Title,
            };
            destination.TripDaysViewModel.Title = source.TripDaysTitle;
            destination.TripDaysViewModel.Monday = source.Monday;
            destination.TripDaysViewModel.Tuesday = source.Tuesday;
            destination.TripDaysViewModel.Thursday = source.Thursday;
            destination.TripDaysViewModel.Friday = source.Friday;
            destination.TripDaysViewModel.Wednesday = source.Wednesday;
            destination.TripDaysViewModel.Saturday = source.Saturday;
            destination.TripDaysViewModel.Sunday = source.Sunday;
            destination.TripDaysViewModel.Id = source.TripDaysId;


            return destination;
        }  
        public static explicit operator ViewTripViewModel(Trip source)
        {
            var destination = new ViewTripViewModel
            {
                DepartureTime = source.DepartureTime.ToString(),
                Discount = source.Discount,
                VehicleId = source.VehicleId.Value,
                CanBeScheduled = source.CanBeScheduled,
                DiscountEndDate = source.DiscountEndDate,
                DiscountStartDate = source.DiscountStartDate,
                Id = source.Id.ToString(),
                Title = source.Title,
                
            };

            return destination;
        }
    } 
    
    public class TripViewModel 
    {
        public string DepartureTime { get; set; }
        public Guid RouteId { get; set; }
        //public decimal BaseFee { get; set; }
        //public decimal DispatchFee { get; set; }
        //public string Title { get; set; }

        public decimal? Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        public bool CanBeScheduled { get; set; }
        public int? NoOfTrips { get; set; }

     //  public Guid Route { get; set; }
        public Guid VehicleId { get; set; }
        public string VehicleModelTitle { get; set; }

        public Guid TripDaysId { get; set; }

      //  public string RouteName { get; set; }
      //  public int RouteNo { get; set; }


        public static explicit operator TripViewModel(TripDto source)
        {
            var destination = new TripViewModel
            {
                DepartureTime = source.DepartureTime,
                Discount = source.Discount
                //
                //et al
            };

            return destination;
        }
    }

    public class EditTripViewModel : BaseViewModel
    {
        public Guid RouteId { get; set; }
        public string DepartureTime { get; set; }
        public string Title { get; set; }
        public DateTime? ScheduledTripTime { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DispatchFee { get; set; }
        public decimal? ChildrenFee { get; set; }

        public decimal? Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }

        public bool CanBeScheduled { get; set; }
        public int? NoOfTrips { get; set; }

        public Guid VehicleId { get; set; }
        public string VehicleModelTitle { get; set; }
        public DateTime? LastTripCreationDate { get; set; }
        public bool IsActive { get; set; }
        public Guid TripDaysId { get; set; }
    }

    public class TripDiscountViewModel : BaseViewModel
    {
        public decimal? Discount { get; set; }
        public DateTime? DiscountStartDate { get; set; }
        public DateTime? DiscountEndDate { get; set; }
    }

    public class EditTripChildrenFeeViewModel : BaseViewModel
    {
        public decimal? ChildrenFee { get; set; }
    }

    public class DeleteTripViewModel : BaseViewModel
    {
    }

    public class DeleteTripChildrenFeeViewModel : BaseViewModel
    {
    }

    public class DeleteTripDiscountViewModel : BaseViewModel
    {
    }


}

