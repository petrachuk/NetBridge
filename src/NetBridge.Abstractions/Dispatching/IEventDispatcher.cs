using NetBridge.Abstractions.Events;

namespace NetBridge.Abstractions.Dispatching
{
    public interface IEventDispatcher
    {
        Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : class, IEvent;
    }
}
