using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Enums;

namespace Booking.Core.Events.EventHandlers
{
    public class UserUpdatedEventHandler : IIntegrationEventHandler<UserUpdatedIntegrationEvent>
    {
        private readonly ICaptainService _driverSvc;
        private readonly IBusBoyService _busboySvc;

        public UserUpdatedEventHandler(ICaptainService driverSvc)
        {
            _driverSvc = driverSvc;
        }

        public Task Handle(UserUpdatedIntegrationEvent @event)
        {
            throw new NotImplementedException();

            //if (@event.UserType == (int)UserType.CAPTAIN)
            //{
            //    _driverSvc.Add(new Captain
            //    {
            //        CreatedBy = @event.CreatedBy,
            //        EmployeeCode = @event.EmployeeCode,
            //        UserId = @event.UserId,
            //        FirstName = @event.FirstName,
            //        LastName = @event.LastName
            //    });
            //}
            //if (@event.UserType == (int)UserType.BUSBOY)
            //{
            //    _busboySvc.Add(new BusBoy
            //    {
            //        CreatedBy = @event.CreatedBy,
            //        UserId = @event.UserId,
            //        FirstName = @event.FirstName,
            //        LastName = @event.LastName
            //    });
            //}

            //return Task.CompletedTask;
        }
    }
}