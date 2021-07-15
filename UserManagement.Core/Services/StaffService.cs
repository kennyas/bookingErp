using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;
using Tornado.Shared.Extensions;
using Tornado.Shared.Timing;
using UserManagement.Core.Models;
using UserManagement.Core.Services.Interfaces;
using UserManagement.Core.ViewModels;

namespace UserManagement.Core.Services
{

    public class StaffService : Service<Staff>, IStaffService
    {
        public StaffService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}