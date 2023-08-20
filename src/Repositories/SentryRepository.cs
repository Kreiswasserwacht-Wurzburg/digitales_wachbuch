using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories
{
    public class SentryRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;

        public SentryRepository(DigitalGuardBookDataContext dataContext)
        {
            _dataContext = dataContext;
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
                await _dataContext.Sentries.InsertOneAsync((Sentry)sentry);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            return sentry;
        }
    }
}