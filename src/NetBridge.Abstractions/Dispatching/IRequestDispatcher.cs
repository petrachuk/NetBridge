using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Queries;

namespace NetBridge.Abstractions.Dispatching
{
    public interface IRequestDispatcher
    {
        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : ICommand;

        Task<TResult> QueryAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : class, IQuery<TResult>;
    }
}
