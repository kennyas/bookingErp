﻿using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Core.Services
{
    public class CustomerService : Service<Customer>, ICustomerService
    {
        public CustomerService( 
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}