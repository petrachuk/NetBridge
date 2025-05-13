using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Commands;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Abstractions.Dispatching;

namespace NetBridge.Dispatching.Tests.Dispatching
{
    public class CommandDispatcherTests
    {
        [Fact]
        public async Task Command_Should_Be_Handled()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICommandHandler<TestCommand>, TestCommandHandler>();
            services.AddSingleton<IRequestDispatcher, RequestDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

            await dispatcher.SendAsync(new TestCommand());

            Assert.True(TestCommandHandler.Handled);
        }

        public class TestCommand : ICommand { }

        public class TestCommandHandler : ICommandHandler<TestCommand>
        {
            public static bool Handled = false;

            public Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default)
            {
                Handled = true;
                return Task.CompletedTask;
            }
        }
    }
}
