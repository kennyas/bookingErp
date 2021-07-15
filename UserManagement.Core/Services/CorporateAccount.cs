using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;

namespace UserManagement.Core.Services
{
    public class CorporateAccountService : Service<CorporateAccount>, ICorporateAccountService
    {
        public CorporateAccountService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
