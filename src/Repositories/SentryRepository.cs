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
        private readonly PersonRepository _personRepository;

        public SentryRepository(DigitalGuardBookDataContext dataContext, LogBookRepository logBookRepository, PersonRepository personRepository)
        {
            _dataContext = dataContext;
            _logBookRepository = logBookRepository;
            _personRepository = personRepository;
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
                await _logBookRepository.InsertLogBookEntryAsync("Sentry started", sentry.Start);

                var personList = await _personRepository.AllPersonsAsync();

                foreach (var service in sentry.GuardServices)
                {
                    var person = personList.FirstOrDefault(x => x.Id == service.PersonId);

                    if (person != null)
                    {
                        await _logBookRepository.InsertLogBookEntryAsync($"Guard {person.FirstName} {person.LastName} has started its service.", sentry.Start);
                    }
                }

                foreach (var service in sentry.SupervisorServices)
                {
                    var person = personList.FirstOrDefault(x => x.Id == service.PersonId);

                    if (person != null)
                    {
                        await _logBookRepository.InsertLogBookEntryAsync($"Supervisor {person.FirstName} {person.LastName} has started its service.", sentry.Start);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
            return sentry;
        }

        private async Task<Sentry> GetSentryAsync(string id)
        {

            return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task FinishSentry(string id, DateTimeOffset dateTime)
        {
            try
            {
                var sentry = await GetSentryAsync(id);
                var personList = await _personRepository.AllPersonsAsync();

                foreach (var service in sentry.GuardServices.Where(x => !x.End.HasValue))
                {
                    var person = personList.FirstOrDefault(x => x.Id == service.PersonId);

                    if (person != null)
                    {
                        await _logBookRepository.InsertLogBookEntryAsync($"Guard {person.FirstName} {person.LastName} has finished its service.", dateTime);
                    }
                }

                foreach (var service in sentry.SupervisorServices.Where(x => !x.End.HasValue))
                {
                    var person = personList.FirstOrDefault(x => x.Id == service.PersonId);

                    if (person != null)
                    {
                        await _logBookRepository.InsertLogBookEntryAsync($"Supervisor {person.FirstName} {person.LastName} has finished its service.", dateTime);
                    }
                }

                var fb = Builders<Sentry>.Filter;
                var filter = fb.And(
                    fb.Eq(x => x.Id, id), 
                    fb.ElemMatch(x => x.SupervisorServices, x => !x.End.HasValue), 
                    fb.ElemMatch(x => x.GuardServices, x => !x.End.HasValue)
                );
                UpdateDefinition<Sentry> update = Builders<Sentry>.Update
                    .Set(x => x.End, dateTime)
                    .Set("SupervisorServices.$.End", dateTime)
                    .Set("GuardServices.$.End", dateTime);
                var res = await _dataContext.Sentries.UpdateOneAsync(filter, update);

                await _logBookRepository.InsertLogBookEntryAsync("Sentry finished", dateTime);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }

        public async Task RegisterSentry(string id, DateTimeOffset dateTime)
        {
            try
            {
                var sentry = await GetSentryAsync(id);

                var fb = Builders<Sentry>.Filter;
                var filter = fb.And(
                    fb.Eq(x => x.Id, id), 
                    fb.ElemMatch(x => x.SupervisorServices, x => !x.End.HasValue), 
                    fb.ElemMatch(x => x.GuardServices, x => !x.End.HasValue)
                );
                UpdateDefinition<Sentry> update = Builders<Sentry>.Update
                    .Set(x => x.Registration, dateTime);
                var res = await _dataContext.Sentries.UpdateOneAsync(filter, update);

                await _logBookRepository.InsertLogBookEntryAsync($"Registered at ILS!", dateTime);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.ToString());
            }
        }
    }
}