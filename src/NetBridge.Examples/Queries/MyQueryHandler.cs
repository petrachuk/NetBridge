using NetBridge.Abstractions.Queries;

namespace NetBridge.Examples.Queries
{
    // Обработчик запроса
    public class MyQueryHandler : IQueryHandler<MyQuery, MyQueryResponse>
    {
        public Task<MyQueryResponse> HandleAsync(MyQuery query, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Handling query with parameter: {query.QueryParameter}");
            return Task.FromResult(new MyQueryResponse
            {
                Value = $"Query processed: {query.QueryParameter}"
            });
        }
    }
}
