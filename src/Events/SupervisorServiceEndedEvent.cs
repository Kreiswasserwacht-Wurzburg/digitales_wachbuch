namespace DigitalGuardBook.Events;

public sealed record SupervisorServiceEndedEvent(
    string PersonId,
    DateTimeOffset EndTime
);
