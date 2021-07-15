using System;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Booking.Core.Events
{
    public class UserUpdatedIntegrationEvent : IntegrationEvent
    {
        public string FirstName { get; }
        public string LastName { get; set; }
        public string Password { get; }
        public string UserName { get; }
        public string EmployeeCode { get; }
        public string Email { get; }
        public int UserType { get; }
        public Guid UpdatedBy { get; set; }
        public string UserId { get; set; }

        public UserUpdatedIntegrationEvent(
            string firstname,string lastName, string username, string email,
            string password, int userType, string employeeCode,
            string userId, Guid updatedBy
            )
        {
            FirstName = firstname;
            LastName = lastName;
            UserName = username;
            Email = email;
            Password = password;
            UserType = userType;
            EmployeeCode = employeeCode;
            UserId = userId;
            UpdatedBy = updatedBy;
        }
    }
}