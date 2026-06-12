namespace DigitalGuardBook.Modules.Sentry.Events;

public sealed record SentryStartedEvent(
    DateTimeOffset StartTime,
    IReadOnlyList<string> GuardPersonIds,
    IReadOnlyList<string> SupervisorPersonIds
);
