using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;
using UserManagement.Core.Models;

namespace UserManagement.Core.Dtos
{
    public class DepartmentDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }

        public static explicit operator DepartmentDto(Department source)
        {
            var department = new DepartmentDto() 
            {
                Id = source.Id,
                Description = source.Description,
                CreatedBy = source.CreatedBy.Value,
                Name = source.Name,
                CreatedDate = source.CreatedOn

                //And any other data that needs to be modified from the way it is in the DB
                //i.e
                // for the case of users

                //userDto.FullName = $"{user.Firstname} {user.LastName}"
            };

            return department;
        }
    }
}
