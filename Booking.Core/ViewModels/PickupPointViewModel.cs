using Booking.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class PickupPointViewModel : BaseViewModel
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("latitude")]
        public decimal? Latitude { get; set; }

        [JsonProperty("longitude")]
        public decimal? Longitude { get; set; }

        [JsonProperty("shortDescription")]
        public string ShortDescription { get; set; }

        [JsonProperty("areaId")]
        public string AreaId { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        public static explicit operator PickupPointViewModel(Point source)
        {
            var destination = new PickupPointViewModel
            {
                Id = source.Id.ToString(),
                Title = source.Title,
                Description = source.Description,
                AreaId = source.AreaId.ToString(),
                Latitude = source.Latitude,
                Longitude = source.Longitude,
                CreatedId = source.CreatedBy ?? Guid.Empty,
                ShortDescription = source.ShortDescription                
            };

            return destination;
        }
    }

    public class ListPickupPointViewModel 
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string ShortDescription { get; set; }

        public Guid? AreaId { get; set; }

        public string Description { get; set; }

        public long RowNo { get; set; }
        public int TotalCount { get; set; }

    }
    public class DeletePickupPointViewModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class PickupPointPaginatedViewModel: BaseSearchViewModel
    {
    }   
   

    //use the base search view model
    public class SearchPickupPointViewModel : BaseSearchViewModel
    {
    }
    
    //use the base search view model
    public class GetPickupPointByIdViewModel : BaseSearchViewModel
    {
        [JsonProperty("id")]
       public string Id { get; set; }
    }
}
