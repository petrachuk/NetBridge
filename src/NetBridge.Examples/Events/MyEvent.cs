using NetBridge.Abstractions.Events;

namespace NetBridge.Examples.Events
{
    public class MyEvent : IEvent
    {
        public string? Payload { get; set; }
    }
}
