using Notify.Core.Models;
using Notify.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Services;

namespace Notify.Core.Services
{
    public class NotificationService : Service<GIGMNotification> ,INotificationService
    {
        public NotificationService(
            IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
    }
}
