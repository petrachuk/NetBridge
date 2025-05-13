using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Commands;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Dispatching.Dispatchers;

namespace NetBridge.Dispatching.Tests.Commands
{
    public class SampleCommand : ICommand { }

    public class SampleCommandHandler : ICommandHandler<SampleCommand>
    {
        public bool Handled { get; private set; }

        public Task HandleAsync(SampleCommand command, CancellationToken cancellationToken = default)
        {
            Handled = true;
            return Task.CompletedTask;
        }
    }

    public class CommandTests
    {
        [Fact]
        public async Task Command_Should_Be_Handled()
        {
            var services = new ServiceCollection();

            var handler = new SampleCommandHandler();
            services.AddSingleton<ICommandHandler<SampleCommand>>(handler);
            services.AddSingleton<IRequestDispatcher, RequestDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IRequestDispatcher>();

            await dispatcher.SendAsync(new SampleCommand());

            Assert.True(handler.Handled);
        }
    }
}
