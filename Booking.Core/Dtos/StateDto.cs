using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace Booking.Core.Dtos
{
    public class StateDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public Guid? ModifiedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public long RowNo { get; set; }
        public string Country { get; set; }
        public string StateCode { get; set; }
        public string CountryCode { get; set; }
        public Guid? CountryId { get; set; } 

        public static explicit operator StateDto(State source)
        {
            var state = new StateDto() 
            {
                Id = source.Id,
                Description = source.Description,
                Code = source.Code,
                Name = source.Name,
                CreatedDate = source.CreatedOn,
                CreatedBy = source.CreatedBy               
            };
            return state;
        }

    }
}
