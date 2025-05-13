using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Queries;
using NetBridge.Abstractions.Dispatching;

namespace NetBridge.Dispatching.Dispatchers
{
    // Локальный Dispatcher
    public class RequestDispatcher(IServiceProvider provider) : IRequestDispatcher
    {
        // Обработка команды
        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : ICommand
        {
            var handler = provider.GetRequiredService<ICommandHandler<TCommand>>();
            await handler.HandleAsync(command, cancellationToken);
        }

        // Обработка запроса
        public async Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : class, IQuery<TResult>
        {
            var handler = provider.GetRequiredService<IQueryHandler<TQuery, TResult>>();
            return await handler.HandleAsync(query, cancellationToken);
        }
    }
}
