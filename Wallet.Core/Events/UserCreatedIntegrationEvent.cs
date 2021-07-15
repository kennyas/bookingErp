using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Wallet.Core.Events
{
    public class UserCreatedIntegrationEvent : IntegrationEvent
    {
        public string FirstName { get; }
        public string Password { get; }
        public string LastName { get; set; }
        public string UserName { get; }
        public string EmployeeCode { get; }
        public string Email { get; }
        public int UserType { get; }
        public Guid CreatedBy { get; set; }
        public Guid UserId { get; set; }

        public UserCreatedIntegrationEvent(
            string firstname, string lastName, string username, string email,
            string password,
            int userType, string employeeCode,
            Guid userId, Guid createdBy
            )
        {
            FirstName = firstname;
            UserName = username;
            Email = email;
            Password = password;
            UserType = userType;
            EmployeeCode = employeeCode;
            UserId = userId;
            CreatedBy = createdBy;
            LastName = lastName;
        }
    }
}
