using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.Dtos;

namespace UserManagement.Core.Dtos
{
    public class CustomerDto : BaseDto
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }

        //other customer details
    }
}
