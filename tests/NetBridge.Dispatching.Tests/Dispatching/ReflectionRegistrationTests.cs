using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Commands;
using NetBridge.Dispatching.DependencyInjection;

namespace NetBridge.Dispatching.Tests.Dispatching
{
    public class ReflectionRegistrationTests
    {
        [Fact]
        public void Should_Register_All_Handlers()
        {
            var services = new ServiceCollection();
            services.AddHandlersFromAssembly(typeof(ReflectionRegistrationTests).Assembly);

            var provider = services.BuildServiceProvider();
            var handler = provider.GetService<ICommandHandler<ReflectionCommand>>();

            Assert.NotNull(handler);
        }

        public class ReflectionCommand : ICommand { }

        public class ReflectionCommandHandler : ICommandHandler<ReflectionCommand>
        {
            public Task HandleAsync(ReflectionCommand command, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }
        }
    }
}
