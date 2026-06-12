namespace DigitalGuardBook.Infrastructure;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent domainEvent);
}
