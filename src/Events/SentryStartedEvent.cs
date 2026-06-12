namespace DigitalGuardBook.Events;

public sealed record SentryStartedEvent(
    DateTimeOffset StartTime,
    IReadOnlyList<string> GuardPersonIds,
    IReadOnlyList<string> SupervisorPersonIds
);
