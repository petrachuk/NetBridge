using System.Reflection;
using MassTransit;
using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Queries;
using NetBridge.Messaging.Consumers;

namespace NetBridge.Messaging.DependencyInjection
{
    public static class ConsumerRegistrationExtensions
    {
        public static void AddDispatchingConsumers(this IBusRegistrationConfigurator cfg, Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(a => a.DefinedTypes);

            foreach (var type in types)
            {
                foreach (var iface in type.ImplementedInterfaces)
                {
                    if (iface.IsGenericType)
                    {
                        var def = iface.GetGenericTypeDefinition();

                        if (def == typeof(ICommandHandler<>))
                        {
                            var commandType = iface.GetGenericArguments()[0];
                            var consumerType = typeof(CommandConsumer<>).MakeGenericType(commandType);
                            cfg.AddConsumer(consumerType);
                        }

                        if (def == typeof(IQueryHandler<,>))
                        {
                            var args = iface.GetGenericArguments();
                            var consumerType = typeof(QueryConsumer<,>).MakeGenericType(args);
                            cfg.AddConsumer(consumerType);
                        }
                    }
                }
            }
        }
    }
}
