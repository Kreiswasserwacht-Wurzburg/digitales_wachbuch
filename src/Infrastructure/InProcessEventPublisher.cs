namespace DigitalGuardBook.Infrastructure;

public sealed class InProcessEventPublisher : IEventPublisher
{
    private readonly Dictionary<Type, List<Func<object, Task>>> _handlers = new();

    public void Subscribe<TEvent>(Func<TEvent, Task> handler)
    {
        var key = typeof(TEvent);
        if (!_handlers.TryGetValue(key, out var list))
            _handlers[key] = list = new();
        list.Add(e => handler((TEvent)e));
    }

    public async Task PublishAsync<TEvent>(TEvent domainEvent)
    {
        if (!_handlers.TryGetValue(typeof(TEvent), out var list)) return;
        foreach (var handler in list)
            await handler(domainEvent!);
    }
}
