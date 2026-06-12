namespace DigitalGuardBook.Infrastructure;

public interface ILogBookEventHandler
{
    void Register(InProcessEventPublisher publisher);
}
