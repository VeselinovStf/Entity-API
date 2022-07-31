using Autofac;
using BuildingBlock.Messaging.Abstraction.EventBus;
using BuildingBlock.Messaging.Abstraction.MQ;
using BuildingBlock.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace BuildingBlock.EventBusRabboitMQ.Extensions
{
    public static class ConfigurationExtension
    {
        public static void AddEventBusRabboitMQ(this IServiceCollection services, 
            string connectionString,
            string subscriptionClientName,
            string virtualHost,
            int retryCount = 5)
        {
            services.AddSingleton<IMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

                var factory = new ConnectionFactory()
                {
                    HostName = connectionString,
                    DispatchConsumersAsync = true,
                    VirtualHost = virtualHost
                };           

                return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
            });
         
            services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
            {
                var rabbitMQPersistentConnection = sp.GetRequiredService<IMQPersistentConnection>();

                var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
                var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
        }
    }
}
