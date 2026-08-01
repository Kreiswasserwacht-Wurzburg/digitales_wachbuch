using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public class SentryRepository : ISentryRepository
{
    private readonly DigitalGuardBookDataContext _dataContext;

    public SentryRepository(DigitalGuardBookDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<SentryEntity> GetActiveSentry()
    {
        return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => !x.End.HasValue);
    }

    public virtual async Task InsertSentryAsync(SentryEntity sentry)
    {
        await _dataContext.Sentries.InsertOneAsync((SentryEntity)sentry);
    }

    public virtual async Task<SentryEntity> GetSentryAsync(string id)
    {
        return await _dataContext.Sentries
            .AsQueryable()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public virtual async Task UpdateSentryEndAsync(string id, DateTimeOffset dateTime)
    {
        var fb = Builders<SentryEntity>.Filter;
        var filter = fb.And(
            fb.Eq(x => x.Id, id),
            fb.ElemMatch(x => x.SupervisorServices, x => !x.End.HasValue),
            fb.ElemMatch(x => x.GuardServices, x => !x.End.HasValue)
        );
        var update = Builders<SentryEntity>.Update
            .Set(x => x.End, dateTime)
            .Set("SupervisorServices.$.End", dateTime)
            .Set("GuardServices.$.End", dateTime);

        await _dataContext.Sentries.UpdateOneAsync(filter, update);
    }

    public virtual async Task AddGuardAsync(string sentryId, GuardService guard)
    {
        var fb = Builders<SentryEntity>.Filter;
        var filter = fb.Eq(x => x.Id, sentryId);
        var update = Builders<SentryEntity>.Update
            .Push(x => x.GuardServices, guard);

        await _dataContext.Sentries.UpdateOneAsync(filter, update);
    }

    public virtual async Task RemoveGuardAsync(string sentryId, string personId, DateTimeOffset end)
    {
        var fb = Builders<SentryEntity>.Filter;
        var filter = fb.And(
            fb.Eq(x => x.Id, sentryId),
            fb.ElemMatch(x => x.GuardServices, g => g.PersonId == personId && !g.End.HasValue)
        );
        var update = Builders<SentryEntity>.Update
            .Set("GuardServices.$.End", end);

        await _dataContext.Sentries.UpdateOneAsync(filter, update);
    }
}
