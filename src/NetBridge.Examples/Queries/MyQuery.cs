using NetBridge.Abstractions.Queries;

namespace NetBridge.Examples.Queries
{
    // Запрос
    public class MyQuery : IQuery<MyQueryResponse>
    {
        public string? QueryParameter { get; set; }
    }
}
