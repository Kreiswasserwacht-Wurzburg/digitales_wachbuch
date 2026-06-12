namespace DigitalGuardBook.Events;

public sealed record GuardServiceStartedEvent(
    string PersonId,
    DateTimeOffset StartTime
);
