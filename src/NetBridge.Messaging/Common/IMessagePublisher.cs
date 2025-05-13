using NetBridge.Abstractions.Events;

namespace NetBridge.Messaging.Common
{
    public interface IMessagePublisher
    {
        Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IEvent;
    }
}
