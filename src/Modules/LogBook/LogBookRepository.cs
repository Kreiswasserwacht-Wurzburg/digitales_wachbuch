using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Modules.Station;
using LogBookEntity = DigitalGuardBook.Data.Entities.LogBook;

namespace DigitalGuardBook.Modules.LogBook
{
    public class LogBookRepository : ILogBookRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;
        private readonly StationRepository _stationRepository;

        public LogBookRepository(DigitalGuardBookDataContext dataContext, StationRepository stationRepository)
        {
            _dataContext = dataContext;
            _stationRepository = stationRepository;
        }

        public async Task<LogBookEntity> GetLogBookAsync(string stationId, int year)
        {
            var logBook = await _dataContext.LogBooks
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.StationId == stationId && x.Year == year);

            if (logBook == null)
            {
                logBook = new LogBookEntity
                {
                    Entries = new List<LogBookEntry>(),
                    StationId = stationId,
                    Year = year
                };

                await _dataContext.LogBooks.InsertOneAsync(logBook);
            }

            return logBook;
        }

        private async Task<string> GetLogBookIdByYearAsync(int? year)
        {
            var stationId = (await _stationRepository.GetStationAsync()).Id;
            var id = await _dataContext.LogBooks
            .AsQueryable()
            .Where(x => x.StationId == stationId && x.Year == (year ?? DateTimeOffset.Now.Year))
            .Select(x => x.Id)
            .FirstOrDefaultAsync();

            if (id == null)
            {
                var logBook = new LogBookEntity
                {
                    Entries = new List<LogBookEntry>(),
                    StationId = stationId,
                    Year = year ?? DateTimeOffset.Now.Year
                };

                await _dataContext.LogBooks.InsertOneAsync(logBook);

                id = logBook.Id;
            }

            return id;
        }

        public async Task<List<LogBookEntry>> GetEntriesForLogBookAsync(DateTimeOffset? from, DateTimeOffset? to)
        {
            var logBookId = await GetLogBookIdByYearAsync(from?.Year ?? to?.Year ?? DateTimeOffset.Now.Year);
            var query = _dataContext.LogBooks.AsQueryable().Where(x => x.Id == logBookId)
            .SelectMany(x => x.Entries);

            if (from.HasValue)
            {
                query = query.Where(x => x.Time >= from);
            }
            if (to.HasValue)
            {
                query = query.Where(x => x.Time < to);
            }

            return await query.ToListAsync();
        }

        public async Task InsertLogBookEntryAsync(string message, DateTimeOffset? time)
        {
            var logBookId = await GetLogBookIdByYearAsync(time?.Year ?? DateTimeOffset.Now.Year);

            var entry = new LogBookEntry
            {
                Author = "System",
                Message = message,
                Time = time ?? DateTimeOffset.Now
            };

            var filter = Builders<LogBookEntity>.Filter.Eq(x => x.Id, logBookId);
            var update = Builders<LogBookEntity>.Update.AddToSet(x => x.Entries, entry);

            await _dataContext.LogBooks.UpdateOneAsync(filter, update);
        }
    }
}
