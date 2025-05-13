using MassTransit;
using NetBridge.Abstractions.Commands;

namespace NetBridge.Messaging.Consumers
{
    public class CommandConsumer<TCommand>(ICommandHandler<TCommand> handler) : IConsumer<TCommand>
        where TCommand : class, ICommand
    {
        public async Task Consume(ConsumeContext<TCommand> context)
        {
            await handler.HandleAsync(context.Message, context.CancellationToken);
        }
    }
}
