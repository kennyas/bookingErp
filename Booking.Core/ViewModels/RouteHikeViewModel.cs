using Booking.Core.Dtos;
using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class RouteHikeCreateViewModel : BaseViewModel 
    {
        [Required]
        public Guid RouteId { get; set; }
        [Required]
        public Guid HikeId { get; set; }       
        public string CreatedUser { get; set; }

        public static explicit operator RouteHikeCreateViewModel(RouteHike source) 
        {
            var RouteHike = new RouteHikeCreateViewModel()
            {
                HikeId = source.HikeId,
                Id = source?.Id.ToString(),
                RouteId = source.RouteId,
                CreatedId = source.CreatedBy.Value
            };

            return RouteHike;
        }

        public static explicit operator RouteHikeCreateViewModel(RouteHikeDto source)
        {
            var RouteHidedto = new RouteHikeCreateViewModel()
            {
                HikeId = source.HikeId,
                Id = source?.Id.ToString(),
                RouteId = source.RouteId
            };

            return RouteHidedto;
        }
    }

    public class RouteHikeEditViewModel : BaseViewModel  
    {
        [Required]
        public Guid RouteId { get; set; }
        [Required]
        public Guid HikeId { get; set; }      
        public string ModifiedUser { get; set; }
    }
   
}
