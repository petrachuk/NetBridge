using NetBridge.Abstractions.Commands;

namespace NetBridge.Examples.Commands
{
    public class MyCommandHandler : ICommandHandler<MyCommand>
    {
        public Task HandleAsync(MyCommand command, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Handled command with message: {command.Message}");
            return Task.CompletedTask;
        }
    }
}
