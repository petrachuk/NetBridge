namespace NetBridge.Messaging.Common
{
    public interface IEventConsumer<in TMessage> where TMessage : class
    {
        Task ConsumeAsync(TMessage message, CancellationToken cancellationToken = default);
    }
}
