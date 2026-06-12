namespace DigitalGuardBook.Modules.Sentry.Events;

public sealed record SupervisorServiceStartedEvent(
    string PersonId,
    DateTimeOffset StartTime
);
