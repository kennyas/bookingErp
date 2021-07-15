using Booking.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class VehicleViewModel : BaseViewModel
    {
        [JsonProperty("vehicleModelId")]
        public string VehicleModelId { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("chassisNumber")]
        public string ChassisNumber { get; set; }

        [JsonProperty("noOfSeats")]
        public int NoOfSeats { get; set; }

        [JsonProperty("partnerId")]
        public string PartnerId { get; set; }

        public static explicit operator VehicleViewModel(Vehicle source)
        {
            var destination = new VehicleViewModel
            {
                Id = source.Id.ToString(),
                ChassisNumber = source.ChassisNumber,
                PartnerId = source.PartnerId.ToString(),
               // NoOfSeats = source.NoOfSeats,
                RegistrationNumber = source.RegistrationNumber,
                VehicleModelId = source.VehicleModelId.ToString(),
                CreatedId = source.CreatedBy ?? Guid.Empty
            };
            return destination;
        }
    } 
    
    public class EditVehicleViewModel : BaseViewModel
    {
        [JsonProperty("vehicleModelId")]
        public string VehicleModelId { get; set; }

        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }

        [JsonProperty("chassisNumber")]
        public string ChassisNumber { get; set; }

        [JsonProperty("noOfSeats")]
        public int NoOfSeats { get; set; }

        [JsonProperty("partnerId")]
        public string PartnerId { get; set; }


        public static explicit operator EditVehicleViewModel(Vehicle source)
        {
            var destination = new EditVehicleViewModel
            {
                Id = source.Id.ToString(),
                ChassisNumber = source.ChassisNumber,
                PartnerId = source.PartnerId.ToString(),
                //NoOfSeats = source.NoOfSeats,
                RegistrationNumber = source.RegistrationNumber,
                VehicleModelId = source.VehicleModelId.ToString(),
            };
            return destination;
        }
    }

    public class DeleteVehicleViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class VehiclePaginatedViewModel : BaseSearchViewModel
    {
        
    }

    public class SearchVehicleByChassisNumberViewModel
    {
        [JsonProperty("chassisNumber")]
        public string ChassisNumber { get; set; }
    }

    public class GetVehicleByPartnerIdViewModel : BasePaginatedViewModel
    {
        [JsonProperty("partnerId")]
        public string PartnerId { get; set; }
    }

    public class SearchVehicleByRegistrationNumberViewModel
    {
        [JsonProperty("registrationNumber")]
        public string RegistrationNumber { get; set; }
    }

    public class TripVehicleSearchModel
    {
        public string VehicleModelId { get; set; }
    }

    public class TripVehicleViewModel
    {
        public Guid Id { get; set; }
        public string RegistrationNumber { get; set; }
    }

    public class VehicleAttachedViewModel
    {
        [Required]
        public string vehilceId { get; set; }
    }

    public class CaptainVehicleAttachedViewModel
    {
        [Required]
        public string vehilceId { get; set; }
        public string captainId { get; set; }
    }
    public class BusBoyVehicleAttachedViewModel
    {
        [Required]
        public string vehilceId { get; set; }
        public string busboyId { get; set; }
    }
}
