using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.IntegrationEventLogEF;

namespace UserManagement.Core.Context.EventLog
{
    public class UserManagementIntegrationLogContext : IntegrationEventLogContext
    {
        public UserManagementIntegrationLogContext(DbContextOptions<UserManagementIntegrationLogContext> options) : base(options)
        {

        }
    }
}