using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Events;
using DigitalGuardBook.Infrastructure;

namespace DigitalGuardBook.Repositories
{
    public class SentryRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;
        private readonly IEventPublisher _eventPublisher;

        public SentryRepository(DigitalGuardBookDataContext dataContext, IEventPublisher eventPublisher)
        {
            _dataContext = dataContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<Sentry> GetActiveSentry()
        {
            return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => !x.End.HasValue);
        }

        public async Task<Sentry> StartSentryAsync(Sentry sentry)
        {
            await _dataContext.Sentries.InsertOneAsync((Sentry)sentry);

            await _eventPublisher.PublishAsync(new SentryStartedEvent(
                StartTime: sentry.Start,
                GuardPersonIds: sentry.GuardServices.Select(s => s.PersonId).ToList(),
                SupervisorPersonIds: sentry.SupervisorServices.Select(s => s.PersonId).ToList()
            ));

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
            var sentry = await GetSentryAsync(id);

            var unfinishedGuardIds = sentry.GuardServices
                .Where(x => !x.End.HasValue)
                .Select(x => x.PersonId)
                .ToList();
            var unfinishedSupervisorIds = sentry.SupervisorServices
                .Where(x => !x.End.HasValue)
                .Select(x => x.PersonId)
                .ToList();

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
            await _dataContext.Sentries.UpdateOneAsync(filter, update);

            await _eventPublisher.PublishAsync(new SentryFinishedEvent(
                FinishTime: dateTime,
                UnfinishedGuardPersonIds: unfinishedGuardIds,
                UnfinishedSupervisorPersonIds: unfinishedSupervisorIds
            ));
        }
    }
}