using NetBridge.Abstractions.Queries;

namespace NetBridge.Examples.Queries
{
    // Обработчик запроса
    public class MyQueryHandler : IQueryHandler<MyQuery, string>
    {
        public Task<string> HandleAsync(MyQuery query, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Handling query with parameter: {query.QueryParameter}");
            return Task.FromResult($"Query result: {query.QueryParameter}");
        }
    }
}
