using MassTransit;
using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Queries;
using NetBridge.Abstractions.Dispatching;

namespace NetBridge.Messaging.Dispatchers
{
    public class MessagingRequestDispatcher(IBus bus) : IRequestDispatcher
    {
        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand
        {
            await bus.Publish(command, cancellationToken);
        }

        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : class, IQuery<TResult>
        {
            var client = bus.CreateRequestClient<TQuery>();
            var response = await client.GetResponse<QueryResponse<TResult>>(query, cancellationToken);
            return response.Message.Result;
        }
    }

    // Стандартный контракт обёртки для передачи результатов
    public class QueryResponse<TResult>
    {
        public TResult Result { get; set; } = default!;
    }
}
