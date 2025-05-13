using MassTransit;
using NetBridge.Abstractions.Queries;

namespace NetBridge.Messaging.Consumers
{
    public class QueryConsumer<TQuery, TResult>(IQueryHandler<TQuery, TResult> handler) : IConsumer<TQuery>
        where TQuery : class, IQuery<TResult>
    {
        public async Task Consume(ConsumeContext<TQuery> context)
        {
            var result = await handler.HandleAsync(context.Message, context.CancellationToken);
            if (result != null) await context.RespondAsync(result);
        }
    }
}
