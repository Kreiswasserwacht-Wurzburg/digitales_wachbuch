namespace DigitalGuardBook.Events;

public sealed record GuardServiceEndedEvent(
    string PersonId,
    DateTimeOffset EndTime
);
