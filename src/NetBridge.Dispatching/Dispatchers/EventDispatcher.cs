using NetBridge.Abstractions.Dispatching;
using NetBridge.Abstractions.Events;
using NetBridge.Messaging.Common;

namespace NetBridge.Dispatching.Dispatchers
{
    public class EventDispatcher(IMessagePublisher publisher) : IEventDispatcher
    {
        public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent
        {
            return publisher.PublishAsync(@event, cancellationToken);
        }
    }
}
