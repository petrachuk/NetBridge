using NetBridge.Abstractions.Events;

namespace NetBridge.Examples.Events
{
    public class MyEventHandler : IEventHandler<MyEvent>
    {
        public Task HandleAsync(MyEvent @event, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Handled event with payload: {@event.Payload}");
            return Task.CompletedTask;
        }
    }
}
