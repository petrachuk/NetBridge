using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Messaging.Common;
using NetBridge.Messaging.Dispatchers;
using NetBridge.Messaging.Transport.MassTransit;

namespace NetBridge.Messaging.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMessagingDispatchers(this IServiceCollection services)
        {
            services.AddSingleton<IRequestDispatcher, MessagingRequestDispatcher>();
            services.AddSingleton<IEventDispatcher, MessagingEventDispatcher>();
            return services;
        }

        public static IServiceCollection AddMessagingWithRabbitMq(this IServiceCollection services, Action<IBusRegistrationConfigurator>? configure = null)
        {
            services.AddMassTransit(x =>
            {
                configure?.Invoke(x);

                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                    cfg.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                });
            });

            services.AddScoped<IMessagePublisher, MassTransitPublisher>();

            return services;
        }

        public static void AddEventConsumer<TConsumer, TMessage>(this IBusRegistrationConfigurator configurator)
            where TConsumer : class, IConsumer<TMessage>
            where TMessage : class
        {
            configurator.AddConsumer<TConsumer>();
        }
    }
}
