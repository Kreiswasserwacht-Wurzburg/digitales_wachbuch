using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Modules.LogBook.Queries;

public sealed class GetLogBookEntriesQueryHandler
{
    private readonly ILogBookRepository _logBookRepository;

    public GetLogBookEntriesQueryHandler(ILogBookRepository logBookRepository)
    {
        _logBookRepository = logBookRepository;
    }

    public Task<List<LogBookEntry>> HandleAsync(GetLogBookEntriesQuery query)
        => _logBookRepository.GetEntriesForLogBookAsync(query.From, query.To);
}
