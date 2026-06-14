using Microsoft.Extensions.Logging;
using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Modules.Sentry.Events;
using DigitalGuardBook.Infrastructure;
using DigitalGuardBook.Modules.Person;
using DigitalGuardBook.Modules.Organisation;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public class SentryService : ISentryService
{
    private readonly ISentryRepository _sentryRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<SentryService> _logger;
    private readonly IPersonRepository _personRepository;
    private readonly IOrganisationRepository _organisationRepository;

    public SentryService(
        ISentryRepository sentryRepository,
        IEventPublisher eventPublisher,
        ILogger<SentryService> logger,
        IPersonRepository personRepository,
        IOrganisationRepository organisationRepository)
    {
        _sentryRepository = sentryRepository;
        _eventPublisher = eventPublisher;
        _logger = logger;
        _personRepository = personRepository;
        _organisationRepository = organisationRepository;
    }

    public async Task<SentryEntity> StartSentryAsync(SentryEntity sentry)
    {
        _logger.LogInformation("Starting sentry {SentryId} with {GuardCount} guards and {SupervisorCount} supervisors",
            sentry.Id, sentry.GuardServices.Count, sentry.SupervisorServices.Count);

        var activeSentry = await _sentryRepository.GetActiveSentry();
        if (activeSentry != null)
            throw new InvalidOperationException(
                $"Cannot start a new sentry while sentry '{activeSentry.Id}' is active. " +
                $"Finish the active sentry first.");

        await ValidateStartSentryAsync(sentry);

        await _sentryRepository.InsertSentryAsync(sentry);
        _logger.LogInformation("Sentry {SentryId} started successfully", sentry.Id);

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
        _logger.LogInformation("Finishing sentry {SentryId}", id);

        var sentry = await _sentryRepository.GetSentryAsync(id);
        if (sentry == null)
            throw new SentryNotFoundException(id);

        var unfinishedGuards = sentry.GuardServices.Where(x => !x.End.HasValue).ToList();
        var unfinishedSupervisors = sentry.SupervisorServices.Where(x => !x.End.HasValue).ToList();

        await _sentryRepository.UpdateSentryEndAsync(id, dateTime);
        _logger.LogInformation("Sentry {SentryId} finished with {UnfinishedGuardCount} unfinished guards and {UnfinishedSupervisorCount} unfinished supervisors",
            id, unfinishedGuards.Count, unfinishedSupervisors.Count);

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

    private async Task ValidateStartSentryAsync(SentryEntity sentry)
    {
        if (sentry.SupervisorServices.Count == 0)
            throw new InvalidOperationException("At least one supervisor must be assigned to a sentry.");

        var organisation = await _organisationRepository.OrganisationAsync(sentry.OrganisationId);
        if (organisation == null)
            throw new OrganisationNotFoundException(sentry.OrganisationId);

        var allPersonIds = sentry.GuardServices
            .Select(s => s.PersonId)
            .Concat(sentry.SupervisorServices.Select(s => s.PersonId))
            .Distinct()
            .ToList();

        var persons = await _personRepository.PersonsAsync(allPersonIds);
        foreach (var personId in allPersonIds)
        {
            if (!persons.Any(p => p.Id == personId))
                throw new PersonNotFoundException(personId);
        }
    }
}
