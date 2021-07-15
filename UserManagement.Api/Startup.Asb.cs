using Autofac;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Tornado.Shared.AzurePub.EventBus;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.ServiceBus;

namespace UserManagement.Api
{
    public partial class Startup
    {

        public void ConfigureIntegrationService(IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IServiceBusPersisterConnection>(sp =>
                {

                    var serviceBusConnectionString = Configuration["EventBusConnection"] + ";EntityPath=" + Configuration["EntityPath"];
                    var logger = sp.GetRequiredService<ILogger<DefaultServiceBusPersisterConnection>>();

                    var serviceBusConnection = new ServiceBusConnectionStringBuilder(serviceBusConnectionString);

                    return new DefaultServiceBusPersisterConnection(serviceBusConnection, logger);
                });
            }
            RegisterEventBus(services);
        }

        private void RegisterEventBus(IServiceCollection services)
        {
            var subscriptionClientName = Configuration["SubscriptionClientName"];

            if (Configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton<IEventBus, EventBusServiceBus>(sp =>
                {
                    var serviceBusPersisterConnection = sp.GetRequiredService<IServiceBusPersisterConnection>();

                    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();



                    var logger = sp.GetRequiredService<ILogger<EventBusServiceBus>>();
                    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                    return new EventBusServiceBus(serviceBusPersisterConnection, logger,
                        eventBusSubcriptionsManager, subscriptionClientName, iLifetimeScope);
                });
            }

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            // services.AddTransient<ProductPriceChangedIntegrationEventHandler>();
        }
    }
}
