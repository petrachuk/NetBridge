using Microsoft.Extensions.DependencyInjection;
using NetBridge.Abstractions.Dispatching;
using NetBridge.Abstractions.Events;
using NetBridge.Dispatching.Dispatchers;
using NetBridge.Messaging.Common;


namespace NetBridge.Dispatching.Tests.Events
{
    public class SampleEvent : IEvent { }

    public class SampleEventHandler : IEventHandler<SampleEvent>
    {
        public static List<string> Logs = [];

        public Task HandleAsync(SampleEvent @event, CancellationToken cancellationToken = default)
        {
            Logs.Add("Handled");
            return Task.CompletedTask;
        }
    }

    public class EventTests
    {
        [Fact]
        public async Task Event_Should_Be_Handled()
        {
            SampleEventHandler.Logs.Clear();

            var services = new ServiceCollection();
            services.AddSingleton<IEventHandler<SampleEvent>, SampleEventHandler>();
            services.AddSingleton<IMessagePublisher, InMemoryMessagePublisher>();
            services.AddSingleton<IEventDispatcher, EventDispatcher>();

            var provider = services.BuildServiceProvider();
            var dispatcher = provider.GetRequiredService<IEventDispatcher>();

            await dispatcher.PublishAsync(new SampleEvent());

            Assert.Contains("Handled", SampleEventHandler.Logs);
        }
    }
}
