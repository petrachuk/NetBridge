using MassTransit;
using Moq;
using NetBridge.Abstractions.Events;
using NetBridge.Messaging.Dispatchers;

namespace NetBridge.Messaging.Tests.Dispatchers
{
    public class MessagingEventDispatcherTests
    {
        [Fact]
        public async Task PublishAsync_ShouldCallBusPublish()
        {
            // Arrange
            var busMock = new Mock<IBus>();
            var dispatcher = new MessagingEventDispatcher(busMock.Object);

            var testEvent = new TestEvent();

            // Act
            await dispatcher.PublishAsync(testEvent);

            // Assert
            busMock.Verify(b => b.Publish(testEvent, CancellationToken.None), Times.Once);
        }

        private class TestEvent : IEvent { }
    }
}
