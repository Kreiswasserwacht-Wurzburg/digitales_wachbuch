using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Modules.Sentry.Events;
using DigitalGuardBook.Infrastructure;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public interface ISentryService
{
    Task<SentryEntity> StartSentryAsync(SentryEntity sentry);
    Task FinishSentryAsync(string id, DateTimeOffset dateTime);
}

public class SentryService : ISentryService
{
    private readonly ISentryRepository _sentryRepository;
    private readonly IEventPublisher _eventPublisher;

    public SentryService(ISentryRepository sentryRepository, IEventPublisher eventPublisher)
    {
        _sentryRepository = sentryRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<SentryEntity> StartSentryAsync(SentryEntity sentry)
    {
        var activeSentry = await _sentryRepository.GetActiveSentry();
        if (activeSentry != null)
            throw new InvalidOperationException(
                $"Cannot start a new sentry while sentry '{activeSentry.Id}' is active. " +
                $"Finish the active sentry first.");

        await _sentryRepository.InsertSentryAsync(sentry);

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

    public async Task FinishSentryAsync(string id, DateTimeOffset dateTime)
    {
        var sentry = await _sentryRepository.GetSentryAsync(id);
        if (sentry == null)
            throw new SentryNotFoundException(id);

        var unfinishedGuards = sentry.GuardServices.Where(x => !x.End.HasValue).ToList();
        var unfinishedSupervisors = sentry.SupervisorServices.Where(x => !x.End.HasValue).ToList();

        await _sentryRepository.UpdateSentryEndAsync(id, dateTime);

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
