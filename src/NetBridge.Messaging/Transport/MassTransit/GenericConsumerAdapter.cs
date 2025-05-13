using MassTransit;
using NetBridge.Messaging.Common;

namespace NetBridge.Messaging.Transport.MassTransit
{
    public class GenericConsumerAdapter<TMessage>(IEventConsumer<TMessage> handler) : IConsumer<TMessage>
        where TMessage : class
    {
        public Task Consume(ConsumeContext<TMessage> context)
        {
            return handler.ConsumeAsync(context.Message, context.CancellationToken);
        }
    }
}
