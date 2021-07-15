using Autofac;
using MassTransit;
using MassTransit.Azure.ServiceBus.Core;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Tornado.Shared.AzurePub.EventBus.Abstractions;
using Tornado.Shared.AzurePub.EventBus.Events;

namespace Tornado.Shared.AzurePub.ServiceBus
{
    public class MassTrasitOverAzServiceBus : IEventBus
    {
        private readonly ILifetimeScope _autofac;
        private readonly string AUTOFAC_SCOPE_NAME = "eshop_event_bus";
        private const string INTEGRATION_EVENT_SUFFIX = "IntegrationEvent";
        private readonly SubscriptionClient _subscriptionClient;
        private readonly IServiceBusPersisterConnection _serviceBusPersisterConnection;

        public MassTrasitOverAzServiceBus(IServiceBusPersisterConnection serviceBusPersisterConnection, ILifetimeScope autofac,
            string subscriptionClientName)
        {
            _serviceBusPersisterConnection = serviceBusPersisterConnection;
            _subscriptionClient = new SubscriptionClient(serviceBusPersisterConnection.ServiceBusConnectionStringBuilder, subscriptionClientName);
            _autofac = autofac;
        }
        public void Publish(IntegrationEvent @event)
        {
            var eventName = @event.GetType().Name.Replace(INTEGRATION_EVENT_SUFFIX, "");
            var jsonMessage = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(jsonMessage);
            var busControl = Bus.Factory.CreateUsingAzureServiceBus(cfg => cfg.Host("localhost"));
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void SubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            throw new NotImplementedException();
        }

        public void UnsubscribeDynamic<TH>(string eventName) where TH : IDynamicIntegrationEventHandler
        {
            throw new NotImplementedException();
        }
    }
}
