using DigitalGuardBook.Data.Entities;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public interface ISentryService
{
    Task<SentryEntity> StartSentryAsync(SentryEntity sentry);
    Task FinishSentryAsync(string id, DateTimeOffset dateTime);
    Task<SentryEntity> AddGuardAsync(string sentryId, GuardService guard);
    Task<SentryEntity> RemoveGuardAsync(string sentryId, string personId, DateTimeOffset end);
}
