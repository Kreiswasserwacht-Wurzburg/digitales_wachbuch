namespace DigitalGuardBook.Events;

public sealed record SupervisorServiceStartedEvent(
    string PersonId,
    DateTimeOffset StartTime
);
