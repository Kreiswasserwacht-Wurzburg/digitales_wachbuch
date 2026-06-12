namespace DigitalGuardBook.Modules.Sentry.Events;

public sealed record GuardServiceEndedEvent(
    string PersonId,
    DateTimeOffset EndTime
);
