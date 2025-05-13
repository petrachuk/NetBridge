using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Events;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Messaging.Common;

namespace NetBridge.Dispatching.Tests.Dispatching
{
    public class MultipleEventHandlersTests
    {
        [Fact]
        public async Task All_Event_Handlers_Should_Be_Invoked()
        {
            var services = new ServiceCollection();
            services.AddTransient<IEventHandler<TestEvent>, FirstHandler>();
            services.AddTransient<IEventHandler<TestEvent>, SecondHandler>();
            services.AddSingleton<IMessagePublisher, InMemoryMessagePublisher>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IEventDispatcher>();

            await dispatcher.PublishAsync(new TestEvent());

            Assert.True(FirstHandler.Handled);
            Assert.True(SecondHandler.Handled);
        }

        public class TestEvent : IEvent { }

        public class FirstHandler : IEventHandler<TestEvent>
        {
            public static bool Handled;
            public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
            {
                Handled = true;
                return Task.CompletedTask;
            }
        }

        public class SecondHandler : IEventHandler<TestEvent>
        {
            public static bool Handled;
            public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
            {
                Handled = true;
                return Task.CompletedTask;
            }
        }
    }
}
