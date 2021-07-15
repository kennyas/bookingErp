using Booking.Core.Dtos;
using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.ViewModels;

namespace Booking.Core.ViewModels
{
    public class RouteViewModel //: BaseViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public string Name { get; set; }

        public Guid? ModifiedBy { get; set; }
        public Guid? CreatedBy { get; set; }
 
        public string RouteDetails { get; set; }

        public static explicit operator RouteViewModel(Route source)
        {
            var destination = new RouteViewModel()
            {
                Id = source.Id.ToString(),
                Name = source.Name,
                Description = source.Description,
                ShortDescription = source.ShortDescription,
                RouteDetails = $"{source.Name} - {source.ShortDescription}" ,
                CreatedBy = source.CreatedBy,
                ModifiedBy = source.ModifiedBy,
            };
            return destination;
        }
        public static explicit operator RouteViewModel(EditRouteViewModel source)
        {
            var destination = new RouteViewModel()
            {
                Id = source.Id.ToString(),
                CreatedBy = source.CreatedId,
                Name = source.Name,
                Description = source.Description,
                ModifiedBy = source.ModifiedBy,
                ShortDescription = source.ShortDescription
            };

            return destination;
        }
       
    }
}
