using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Events;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Messaging.Common;

namespace NetBridge.Dispatching.Tests.Dispatching
{
    public class EventDispatcherTests
    {
        [Fact]
        public async Task Event_Should_Be_Handled()
        {
            var services = new ServiceCollection();
            services.AddTransient<IEventHandler<TestEvent>, TestEventHandler>();
            services.AddSingleton<IMessagePublisher, InMemoryMessagePublisher>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IEventDispatcher>();

            await dispatcher.PublishAsync(new TestEvent());

            Assert.True(TestEventHandler.Handled);
        }

        public class TestEvent : IEvent { }

        public class TestEventHandler : IEventHandler<TestEvent>
        {
            public static bool Handled = false;

            public Task HandleAsync(TestEvent @event, CancellationToken cancellationToken = default)
            {
                Handled = true;
                return Task.CompletedTask;
            }
        }
    }
}
