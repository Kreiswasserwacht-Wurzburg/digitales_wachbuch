using DigitalGuardBook.Data.Entities;
using LogBookEntity = DigitalGuardBook.Data.Entities.LogBook;

namespace DigitalGuardBook.Modules.LogBook;

public interface ILogBookRepository
{
    Task<LogBookEntity> GetLogBookAsync(string stationId, int year);
    Task<List<LogBookEntry>> GetEntriesForLogBookAsync(DateTimeOffset? from, DateTimeOffset? to);
    Task InsertLogBookEntryAsync(string message, DateTimeOffset? time);
}
