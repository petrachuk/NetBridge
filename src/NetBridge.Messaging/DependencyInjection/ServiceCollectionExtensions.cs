using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MassTransit;
using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Abstractions.Events;
using NetBridge.Messaging.Common;
using NetBridge.Messaging.Dispatchers;
using NetBridge.Messaging.Transport.MassTransit;
using NetBridge.Messaging.Configuration;
using NetBridge.Messaging.Consumers;
using NetBridge.Abstractions.Queries;

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

        public static IServiceCollection AddMessagingWithRabbitMq(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator>? configure = null)
        {
            // Привязка опций из конфигурации
            services.Configure<MessagingOptions>(configuration.GetSection("Messaging"));

            // Конфигурация RabbitMQ
            services.AddMassTransit(x =>
            {
                configure?.Invoke(x);

                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    var options = context.GetRequiredService<IOptions<MessagingOptions>>().Value;

                    // Проверка на наличие переменной окружения для пароля
                    var passwordFromEnv = Environment.GetEnvironmentVariable("RMQ_PASS");
                    if (!string.IsNullOrEmpty(passwordFromEnv)) options.Password = passwordFromEnv;

                    cfg.ConfigureEndpoints(context);
                    cfg.Host(options.Host, options.VirtualHost, h =>
                    {
                        h.Username(options.Username);
                        h.Password(options.Password);
                    });
                });
            });

            services.AddScoped<IMessagePublisher, MassTransitPublisher>();

            return services;
        }

        public static void AddDispatchingCommand<TCommand, THandler>(this IBusRegistrationConfigurator configurator)
            where TCommand : class, ICommand
            where THandler : class, ICommandHandler<TCommand>
        {
            configurator.AddConsumer<CommandConsumer<TCommand>>();
            configurator.AddScoped<ICommandHandler<TCommand>, THandler>();
        }

        public static void AddDispatchingQuery<TQuery, TResult, THandler>(this IBusRegistrationConfigurator configurator)
            where TQuery : class, IQuery<TResult>
            where THandler : class, IQueryHandler<TQuery, TResult>
        {
            configurator.AddConsumer<QueryConsumer<TQuery, TResult>>();
            configurator.AddScoped<IQueryHandler<TQuery, TResult>, THandler>();
        }

        public static void AddDispatchingEvent<TEvent, THandler>(
            this IBusRegistrationConfigurator configurator)
            where TEvent : class, IEvent
            where THandler : class, IEventHandler<TEvent>
        {
            configurator.AddConsumer<EventConsumer<TEvent>>();
            configurator.AddScoped<IEventHandler<TEvent>, THandler>();
        }
    }
}
