namespace DigitalGuardBook.Events;

public sealed record SentryFinishedEvent(
    DateTimeOffset FinishTime,
    IReadOnlyList<string> UnfinishedGuardPersonIds,
    IReadOnlyList<string> UnfinishedSupervisorPersonIds
);
