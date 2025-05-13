using MassTransit;
using Moq;
using NetBridge.Messaging.Dispatchers;
using NetBridge.Abstractions.Commands;

namespace NetBridge.Messaging.Tests.Dispatchers
{
    public class MessagingRequestDispatcherTests
    {
        [Fact]
        public async Task SendAsync_ShouldCallPublishOnEndpoint()
        {
            // Arrange
            var busMock = new Mock<IBus>();

            busMock
                .Setup(b => b.Publish(It.IsAny<ICommand>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask); // имитируем успешную отправку

            var dispatcher = new MessagingRequestDispatcher(busMock.Object);
            var command = new TestCommand();

            // Act
            await dispatcher.SendAsync(command, CancellationToken.None);

            // Assert
            busMock.Verify(p => p.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        private class TestCommand : ICommand { }
    }
}
