using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Queries;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Abstractions.Dispatching;

namespace NetBridge.Dispatching.Tests.Dispatching
{
    public class QueryDispatcherTests
    {
        [Fact]
        public async Task Query_Should_Return_Result()
        {
            var services = new ServiceCollection();
            services.AddTransient<IQueryHandler<TestQuery, string>, TestQueryHandler>();
            services.AddSingleton<IRequestDispatcher, RequestDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

            var result = await dispatcher.QueryAsync<TestQuery, string>(new TestQuery());

            Assert.Equal("result", result);
        }

        public class TestQuery : IQuery<string> { }

        public class TestQueryHandler : IQueryHandler<TestQuery, string>
        {
            public Task<string> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
            {
                return Task.FromResult("result");
            }
        }
    }
}
