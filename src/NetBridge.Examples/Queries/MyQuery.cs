using NetBridge.Abstractions.Queries;

namespace NetBridge.Examples.Queries
{
    // Запрос
    public class MyQuery : IQuery<string>
    {
        public string? QueryParameter { get; set; }
    }
}
