using NetBridge.Abstractions.Commands;

namespace NetBridge.Examples.Commands
{
    // Команда
    public class MyCommand : ICommand
    {
        public string? Message { get; set; }
    }
}
