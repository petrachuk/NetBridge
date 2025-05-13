using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Abstractions.Queries;
using NetBridge.Dispatching.Dispatchers;

namespace NetBridge.Dispatching.Tests.Queries
{
    public class SampleQuery : IQuery<string> { }

    public class SampleQueryHandler : IQueryHandler<SampleQuery, string>
    {
        public Task<string> HandleAsync(SampleQuery query, CancellationToken cancellationToken = default)
            => Task.FromResult("result");
    }

    public class QueryTests
    {
        [Fact]
        public async Task Query_Should_Return_Result()
        {
            var services = new ServiceCollection();

            services.AddSingleton<IQueryHandler<SampleQuery, string>, SampleQueryHandler>();
            services.AddSingleton<IRequestDispatcher, RequestDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

            var result = await dispatcher.QueryAsync<SampleQuery, string>(new SampleQuery());

            Assert.Equal("result", result);
        }
    }
}
