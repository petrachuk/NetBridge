using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Commands;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Abstractions.Dispatching;

namespace NetBridge.Dispatching.Tests.Dispatching
{
    public class ErrorHandlingTests
    {
        [Fact]
        public async Task Missing_Handler_Should_Throw()
        {
            var services = new ServiceCollection();
            services.AddSingleton<IRequestDispatcher, RequestDispatcher>();
            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                dispatcher.SendAsync(new DummyCommand())
            );
        }

        public class DummyCommand : ICommand { }
    }
}
