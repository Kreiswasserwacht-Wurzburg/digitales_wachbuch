namespace DigitalGuardBook.Modules.Sentry.Events;

public sealed record SupervisorServiceEndedEvent(
    string PersonId,
    DateTimeOffset EndTime
);
