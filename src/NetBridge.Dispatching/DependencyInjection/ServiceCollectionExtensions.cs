using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Abstractions.Events;
using NetBridge.Abstractions.Queries;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Messaging.Common;

namespace NetBridge.Dispatching.DependencyInjection
{
    /// <summary>
    /// Extension methods for adding command handlers to the service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Регистрирует инфраструктурные сервисы для обработки команд, запросов и событий.
        /// </summary>
        public static IServiceCollection AddDispatching(this IServiceCollection services)
        {
            services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.AddSingleton<IMessagePublisher, InMemoryMessagePublisher>();

            return services;
        }

        /// <summary>
        /// Регистрирует обработчик команды.
        /// </summary>
        public static IServiceCollection AddCommandHandler<TCommand, THandler>(this IServiceCollection services)
            where TCommand : class, ICommand
            where THandler : class, ICommandHandler<TCommand>
        {
            services.AddTransient<ICommandHandler<TCommand>, THandler>();
            return services;
        }

        /// <summary>
        /// Регистрирует обработчик запроса.
        /// </summary>
        public static IServiceCollection AddQueryHandler<TQuery, TResult, THandler>(this IServiceCollection services)
            where TQuery : class, IQuery<TResult>
            where THandler : class, IQueryHandler<TQuery, TResult>
        {
            services.AddTransient<IQueryHandler<TQuery, TResult>, THandler>();
            return services;
        }

        /// <summary>
        /// Регистрирует обработчик события.
        /// </summary>
        public static IServiceCollection AddEventHandler<TEvent, THandler>(this IServiceCollection services)
            where TEvent : class, IEvent
            where THandler : class, IEventHandler<TEvent>
        {
            services.AddTransient<IEventHandler<TEvent>, THandler>();
            return services;
        }

        /// <summary>
        /// Автоматически регистрирует все обработчики команд, запросов и событий из указанной сборки.
        /// </summary>
        public static IServiceCollection AddHandlersFromAssembly(this IServiceCollection services, Assembly assembly)
        {
            var handlerInterfaces = new[]
            {
                typeof(ICommandHandler<>),
                typeof(IQueryHandler<,>),
                typeof(IEventHandler<>)
            };

            var types = assembly
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false });

            foreach (var type in types)
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (!iface.IsGenericType) continue;

                    var generic = iface.GetGenericTypeDefinition();
                    if (handlerInterfaces.Contains(generic))
                    {
                        services.AddTransient(iface, type);
                    }
                }
            }

            return services;
        }
    }
}