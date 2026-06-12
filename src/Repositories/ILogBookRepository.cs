using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories;

public interface ILogBookRepository
{
    Task<LogBook> GetLogBookAsync(string stationId, int year);
    Task<List<LogBookEntry>> GetEntriesForLogBookAsync(DateTimeOffset? from, DateTimeOffset? to);
    Task InsertLogBookEntryAsync(string message, DateTimeOffset? time);
}
