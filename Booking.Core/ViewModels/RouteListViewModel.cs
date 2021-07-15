using Booking.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Booking.Core.ViewModels
{
    public class RouteListViewModel
    {
        public string Id { get; set; }

        public string Description { get; set; }
        public string ShortDescription { get; set; }

        public string Name { get; set; }

        public string NewName { get; set; }
        public int RouteNo { get; set; }

        public Guid? ModifiedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public int TotalCount { get; set; }

        public long RowNo { get; set; }

        public string RouteDetails { get; set; }

        public decimal BaseFee { get; set; }

        public decimal DispatchFee { get; set; }

        public static explicit operator RouteListViewModel(RouteDto source)
        {
            var destination = new RouteListViewModel()
            {
                Id = source.Id.ToString(),
                Name = source.Name,
                Description = source.Description,
                TotalCount = source.TotalCount,
                RowNo = source.RowNo,
                ModifiedBy = source.ModifiedBy,
                CreatedBy = source.CreatedBy,
                BaseFee = source.BaseFee,
                DispatchFee = source.DispatchFee
                //etc
                //todo
                //user details
                //destination pick up details
                //departure pick up details
            };
            return destination;
        }
    }
}
