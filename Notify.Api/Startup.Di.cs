using Microsoft.Extensions.DependencyInjection;
using Notify.Core.Context;
using Notify.Core.Services;
using Notify.Core.Services.Interfaces;
using Tornado.Shared.EF;
using Tornado.Shared.EF.Repository;
using Tornado.Shared.Messaging.Email;
using Tornado.Shared.Messaging.Notifications;
using Tornado.Shared.Messaging.Sms;

namespace Notify.Api
{
    public partial class Startup
    {
        public void ConfigureDIService(IServiceCollection services)
        {
            services.AddTransient<IWebClient, WebClient>();
            services.AddTransient<IMailService, SmtpEmailService>();
            services.AddTransient<ISmsService, SMSService>();
            services.AddTransient<IFNotification, FNotification>();

            services.AddTransient<INotificationService, NotificationService>();

            services.AddScoped<IDbContext, NotificationContext>();
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
        }
    }
}