namespace DigitalGuardBook.Infrastructure;

public interface ILogBookEventHandler
{
    void Register(IInProcessEventPublisher publisher);
}
