using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Modules.Sentry.Events;
using DigitalGuardBook.Infrastructure;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public class SentryRepository
{
        private readonly DigitalGuardBookDataContext _dataContext;
        private readonly IEventPublisher _eventPublisher;

        public SentryRepository(DigitalGuardBookDataContext dataContext, IEventPublisher eventPublisher)
        {
            _dataContext = dataContext;
            _eventPublisher = eventPublisher;
        }

        public async Task<SentryEntity> GetActiveSentry()
        {
            return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => !x.End.HasValue);
        }

        public async Task<SentryEntity> StartSentryAsync(SentryEntity sentry)
        {
            await _dataContext.Sentries.InsertOneAsync((SentryEntity)sentry);

            await _eventPublisher.PublishAsync(new SentryStartedEvent(
                StartTime: sentry.Start,
                GuardPersonIds: sentry.GuardServices.Select(s => s.PersonId).ToList(),
                SupervisorPersonIds: sentry.SupervisorServices.Select(s => s.PersonId).ToList()
            ));

            foreach (var guardService in sentry.GuardServices)
            {
                await _eventPublisher.PublishAsync(new GuardServiceStartedEvent(
                    PersonId: guardService.PersonId,
                    StartTime: guardService.Start
                ));
            }

            foreach (var supervisorService in sentry.SupervisorServices)
            {
                await _eventPublisher.PublishAsync(new SupervisorServiceStartedEvent(
                    PersonId: supervisorService.PersonId,
                    StartTime: supervisorService.Start
                ));
            }

            return sentry;
        }

        private async Task<SentryEntity> GetSentryAsync(string id)
        {

            return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task FinishSentry(string id, DateTimeOffset dateTime)
        {
            var sentry = await GetSentryAsync(id);

            var unfinishedGuards = sentry.GuardServices
                .Where(x => !x.End.HasValue)
                .ToList();
            var unfinishedSupervisors = sentry.SupervisorServices
                .Where(x => !x.End.HasValue)
                .ToList();

            var fb = Builders<SentryEntity>.Filter;
            var filter = fb.And(
                fb.Eq(x => x.Id, id),
                fb.ElemMatch(x => x.SupervisorServices, x => !x.End.HasValue),
                fb.ElemMatch(x => x.GuardServices, x => !x.End.HasValue)
            );
            UpdateDefinition<SentryEntity> update = Builders<SentryEntity>.Update
                .Set(x => x.End, dateTime)
                .Set("SupervisorServices.$.End", dateTime)
                .Set("GuardServices.$.End", dateTime);
            await _dataContext.Sentries.UpdateOneAsync(filter, update);

            foreach (var guardService in unfinishedGuards)
            {
                await _eventPublisher.PublishAsync(new GuardServiceEndedEvent(
                    PersonId: guardService.PersonId,
                    EndTime: dateTime
                ));
            }

            foreach (var supervisorService in unfinishedSupervisors)
            {
                await _eventPublisher.PublishAsync(new SupervisorServiceEndedEvent(
                    PersonId: supervisorService.PersonId,
                    EndTime: dateTime
                ));
            }

            await _eventPublisher.PublishAsync(new SentryFinishedEvent(
                FinishTime: dateTime,
                UnfinishedGuardPersonIds: unfinishedGuards.Select(x => x.PersonId).ToList(),
                UnfinishedSupervisorPersonIds: unfinishedSupervisors.Select(x => x.PersonId).ToList()
            ));
        }
    }