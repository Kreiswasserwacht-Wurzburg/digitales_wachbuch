using DigitalGuardBook.Data.Entities;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry.Queries;

public sealed class GetActiveSentryQueryHandler
{
    private readonly ISentryRepository _sentryRepository;

    public GetActiveSentryQueryHandler(ISentryRepository sentryRepository)
    {
        _sentryRepository = sentryRepository;
    }

    public Task<SentryEntity?> HandleAsync(GetActiveSentryQuery query)
        => _sentryRepository.GetActiveSentry();
}
