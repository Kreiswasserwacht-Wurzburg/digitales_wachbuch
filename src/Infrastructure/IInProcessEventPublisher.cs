namespace DigitalGuardBook.Infrastructure;

public interface IInProcessEventPublisher : IEventPublisher
{
    void Subscribe<TEvent>(Func<TEvent, Task> handler);
}
