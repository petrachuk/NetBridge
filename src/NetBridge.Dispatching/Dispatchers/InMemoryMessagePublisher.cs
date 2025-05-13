using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Events;
using NetBridge.Messaging.Common;

namespace NetBridge.Dispatching.Dispatchers
{
    public class InMemoryMessagePublisher(IServiceProvider provider) : IMessagePublisher
    {
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            var handlers = provider.GetServices<IEventHandler<TEvent>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }
    }
}
