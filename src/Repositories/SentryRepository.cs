using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using System.Transactions;

namespace DigitalGuardBook.Repositories
{
    public class SentryRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;
        private readonly LogBookRepository _logBookRepository;

        public SentryRepository(DigitalGuardBookDataContext dataContext, LogBookRepository logBookRepository)
        {
            _dataContext = dataContext;
            _logBookRepository = logBookRepository;
        }

        public async Task<Sentry> GetActiveSentry()
        {
            return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => !x.End.HasValue);
        }

        public async Task<Sentry> StartSentryAsync(Sentry sentry)
        {
            try
            {
                var stationTask = _dataContext.Stations.AsQueryable().FirstAsync();
                await _dataContext.Sentries.InsertOneAsync((Sentry)sentry);
                var stationId = (await stationTask).Id;
                var logBook = await _logBookRepository.GetLogBookAsync(stationId, sentry.Start.Year);
                await _logBookRepository.InsertLogBookEntryAsync(logBook.Id, "Sentry started", sentry.Start);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            return sentry;
        }

        public async Task FinishSentry(string id, DateTimeOffset dateTime)
        {
            try
            {
                var stationTask = _dataContext.Stations.AsQueryable().FirstAsync();
                var fb = Builders<Sentry>.Filter;
                var filter = fb.And(fb.Eq(x => x.Id, id), fb.ElemMatch(x => x.SupervisorServices, x => !x.End.HasValue));
                UpdateDefinition<Sentry> update = Builders<Sentry>.Update
                    .Set(x => x.End, dateTime)
                    .Set("SupervisorServices.$.End", dateTime);
                var res = await _dataContext.Sentries.UpdateOneAsync(filter, update);

                var stationId = (await stationTask).Id;
                var logBook = await _logBookRepository.GetLogBookAsync(stationId, dateTime.Year);
                await _logBookRepository.InsertLogBookEntryAsync(logBook.Id, "Sentry finished", dateTime);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}