using Booking.Core.Dtos;
using Booking.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class VehicleMakeViewModel : BaseViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("modifiedBy")]
        public Guid? ModifiedBy { get; set; }

        [JsonProperty("createdBy")]
        public Guid? CreatedBy { get; set; }

        public static explicit operator VehicleMakeViewModel(VehicleMake source)
        {
            var destination = new VehicleMakeViewModel
            {
                Id = source.Id.ToString(),
                CreatedId = source.CreatedBy ?? Guid.Empty,
                Description = source.Description,
                ShortDescription = source.ShortDescription,
                Title = source.Title,
                ModifiedBy = source.ModifiedBy,
                CreatedBy = source.CreatedBy 
            };
            return destination;
        }
    } 
    
    public class EditVehicleMakeViewModel : BaseViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("modifiedBy")]
        public Guid? ModifiedBy { get; set; }

        [JsonProperty("createdBy")]
        public Guid? CreatedBy { get; set; }

        public static explicit operator EditVehicleMakeViewModel(VehicleMake source)
        {
            var destination = new EditVehicleMakeViewModel
            {
                Id = source.Id.ToString(),
                CreatedId = source.CreatedBy ?? Guid.Empty,
                Description = source.Description,
                ShortDescription = source.ShortDescription,
                Title = source.Title,
                ModifiedBy = source.ModifiedBy,
                CreatedBy = source.CreatedBy 
            };
            return destination;
        }
    }

    public class DeleteVehicleMakeViewModel
    {
        [JsonProperty("makeId")]
        public string MakeId { get; set; }
    }   
    
    public class GetVehicleMakeByIdViewModel
    {
        [JsonProperty("makeId")]
        public string MakeId { get; set; }
    }

    //use the base search view model
    public class VehicleMakeSearchViewModel : BaseSearchViewModel
    {
    }

}
