using MassTransit;
using NetBridge.Abstractions.Events;

namespace NetBridge.Messaging.Consumers
{
    public class EventConsumer<TEvent>(IEventHandler<TEvent> handler) : IConsumer<TEvent>
        where TEvent : class, IEvent
    {
        public async Task Consume(ConsumeContext<TEvent> context)
        {
            await handler.HandleAsync(context.Message, context.CancellationToken);
        }
    }
}
