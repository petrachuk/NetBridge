using MassTransit;
using NetBridge.Abstractions.Events;
using NetBridge.Messaging.Common;

namespace NetBridge.Messaging.Transport.MassTransit
{
    public class MassTransitPublisher(IPublishEndpoint publishEndpoint) : IMessagePublisher
    {
        public Task PublishAsync<TMessage>(TMessage message, CancellationToken cancellationToken = default)
            where TMessage : class, IEvent
        {
            return publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
