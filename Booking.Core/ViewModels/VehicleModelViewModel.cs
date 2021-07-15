using Booking.Core.Dtos;
using Booking.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class VehicleModelViewModel : BaseViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("vehicleMakeId")]
        public string VehicleMakeId { get; set; }

        [JsonProperty("vehicleMake")]
        public string VehicleMake { get; set; }

        [JsonProperty("modifiedId")]
        public Guid? ModifiedId { get; set; }

        [JsonProperty("noOfSeats")]
        public int NoOfSeats { get; set; }


        public static explicit operator VehicleModelViewModel(VehicleModelDto source)
        {
            var destination = new VehicleModelViewModel
            {
                Title = source.Title,
                Description = source.Description,
                VehicleMake = source.VehicleMake,
                VehicleMakeId = source.VehicleMakeId.ToString(),
                CreatedId = source.CreatedId.HasValue ? source.CreatedId.Value : Guid.Empty,
                ModifiedId = source.ModifiedBy,
                Id = source.Id.ToString()
            };

            return destination;
        }

        public static explicit operator VehicleModelViewModel(VehicleModel source)
        {
            var destination = new VehicleModelViewModel
            {
                Title = source.Title,
                Description = source.Description,
                VehicleMake = source.VehicleMake?.Title,
                VehicleMakeId = source.VehicleMakeId.ToString(),
                ModifiedId = source.ModifiedBy,
                CreatedId = source.CreatedBy ?? Guid.Empty,
                ShortDescription = source.ShortDescription
            };

            return destination;
        }
        //extra validations
    }   
    
    public class EditVehicleModelViewModel : BaseViewModel
    {
        public string Title { get; set; }
      
        public string Description { get; set; }

        public string ShortDescription { get; set; }

        public string VehicleMakeId { get; set; }

        public string VehicleMake { get; set; }

        public Guid? ModifiedId { get; set; }

        public static explicit operator EditVehicleModelViewModel(VehicleModelDto source)
        {
            var destination = new EditVehicleModelViewModel
            {
                Title = source.Title,
                Description = source.Description,
                VehicleMake = source.VehicleMake,
                VehicleMakeId = source.VehicleMakeId.ToString(),
                CreatedId = source.CreatedId.HasValue ? source.CreatedId.Value : Guid.Empty,
                ModifiedId = source.ModifiedBy,
            };

            return destination;
        }

        public static explicit operator EditVehicleModelViewModel(VehicleModel source)
        {
            var destination = new EditVehicleModelViewModel
            {
                Title = source.Title,
                Description = source.Description,
                VehicleMake = source.VehicleMake?.Title,
                VehicleMakeId = source.VehicleMakeId.ToString(),
                Id = source.Id.ToString(),
                CreatedId = source.CreatedBy ?? Guid.Empty,
                ShortDescription = source.ShortDescription,
                ModifiedId = source.ModifiedBy
            };

            return destination;
        }
        //extra validations
    }

    public class DeleteVehicleModel
    {
        public string ModelId { get; set; }
    }

    public class VehicleModelById
    {
        public string Id { get; set; }
    }

    public class VehicleModelByMake : BasePaginatedViewModel
    {
        public string MakeId { get; set; }
    }

    public class VehicleModelSearch : BaseSearchViewModel
    {
    }
}
