using MassTransit;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Abstractions.Events;

namespace NetBridge.Messaging.Dispatchers
{
    public class MessagingEventDispatcher(IBus bus) : IEventDispatcher
    {
        public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            await bus.Publish(@event, cancellationToken);
        }
    }
}
