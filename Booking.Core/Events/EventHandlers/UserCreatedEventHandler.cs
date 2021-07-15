using Booking.Core.Models;
using Booking.Core.Services.Interfaces;
using System.Threading.Tasks;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.Enums;

namespace Booking.Core.Events.EventHandlers
{
    public class UserCreatedEventHandler : IIntegrationEventHandler<UserCreatedIntegrationEvent>
    {
        private readonly ICaptainService _driverSvc;
        private readonly IBusBoyService _busboySvc;

        public UserCreatedEventHandler(ICaptainService driverSvc, IBusBoyService busboySvc)
        {
            _driverSvc = driverSvc;
            _busboySvc = busboySvc;
        }

        public Task Handle(UserCreatedIntegrationEvent @event)
        {
            if (@event.UserType == (int)UserType.CAPTAIN)
            {
                _driverSvc.Add(new Captain
                {
                    CreatedBy = @event.CreatedBy,
                    //EmployeeCode = @event.EmployeeCode,
                    UserId = @event.UserId,
                    FirstName = @event.FirstName,
                    LastName = @event.LastName
                });
            }
            if (@event.UserType == (int)UserType.BUSBOY)
            {
                _busboySvc.Add(new BusBoy
                {
                    CreatedBy = @event.CreatedBy,
                    BusBoyStatus = Enums.BusBoyStatus.Idle,
                    UserId = @event.UserId,
                    FirstName = @event.FirstName,
                    LastName = @event.LastName
                });
            }

            return Task.CompletedTask;
        }
    }
}