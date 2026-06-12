namespace DigitalGuardBook.Modules.Sentry.Events;

public sealed record GuardServiceStartedEvent(
    string PersonId,
    DateTimeOffset StartTime
);
