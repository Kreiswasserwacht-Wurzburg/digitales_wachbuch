using DigitalGuardBook.Data.Entities;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public interface ISentryRepository
{
    Task<SentryEntity> GetActiveSentry();
    Task InsertSentryAsync(SentryEntity sentry);
    Task<SentryEntity> GetSentryAsync(string id);
    Task UpdateSentryEndAsync(string id, DateTimeOffset dateTime);
    Task AddGuardAsync(string sentryId, GuardService guard);
    Task RemoveGuardAsync(string sentryId, string personId, DateTimeOffset end);
}
