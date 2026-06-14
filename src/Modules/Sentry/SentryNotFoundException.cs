namespace DigitalGuardBook.Modules.Sentry;

public sealed class SentryNotFoundException : InvalidOperationException
{
    public SentryNotFoundException(string sentryId)
        : base($"Sentry with ID '{sentryId}' not found.")
    {
        SentryId = sentryId;
    }

    public string SentryId { get; }
}
