using Booking.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace Booking.Core.Dtos
{
    public class CountryDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public long RowNo { get; set; }

        public static explicit operator CountryDto(Country source)
        {
            var destination = new CountryDto()
            {
                Id = source.Id,
                Description = source.Description,
                Code = source.Code,
                Name = source.Name,
                CreatedDate = source.CreatedOn,
                CreatedBy  = source.CreatedBy
                //And any other data that needs to be modified from the way it is in the DB
                //i.e
                // for the case of users
                //userDto.FullName = $"{user.Firstname} {user.LastName}"
            };
            return destination;
        }
    }
}
