using DigitalGuardBook.Modules.Sentry.Events;
using DigitalGuardBook.Infrastructure;
using DigitalGuardBook.Repositories;
using Microsoft.Extensions.Localization;

namespace DigitalGuardBook.Modules.Sentry.EventHandlers;

public sealed class SentryLogBookEventHandler : ILogBookEventHandler
{
    private readonly LogBookRepository _logBookRepository;
    private readonly PersonRepository _personRepository;
    private readonly IStringLocalizer<SentryLogBookEventHandler> _localizer;

    public SentryLogBookEventHandler(
        LogBookRepository logBookRepository,
        PersonRepository personRepository,
        IStringLocalizer<SentryLogBookEventHandler> localizer)
    {
        _logBookRepository = logBookRepository;
        _personRepository = personRepository;
        _localizer = localizer;
    }

    public void Register(InProcessEventPublisher publisher)
    {
        publisher.Subscribe<SentryStartedEvent>(HandleSentryStartedAsync);
        publisher.Subscribe<SentryFinishedEvent>(HandleSentryFinishedAsync);
        publisher.Subscribe<GuardServiceStartedEvent>(HandleGuardServiceStartedAsync);
        publisher.Subscribe<GuardServiceEndedEvent>(HandleGuardServiceEndedAsync);
        publisher.Subscribe<SupervisorServiceStartedEvent>(HandleSupervisorServiceStartedAsync);
        publisher.Subscribe<SupervisorServiceEndedEvent>(HandleSupervisorServiceEndedAsync);
    }

    private async Task HandleSentryStartedAsync(SentryStartedEvent e)
    {
        await _logBookRepository.InsertLogBookEntryAsync(_localizer["SentryStarted"], e.StartTime);

        var allIds = e.GuardPersonIds.Concat(e.SupervisorPersonIds).Distinct().ToList();
        var persons = await _personRepository.PersonsAsync(allIds);

        foreach (var id in e.GuardPersonIds)
        {
            var person = persons.FirstOrDefault(x => x.Id == id);
            if (person != null)
                await _logBookRepository.InsertLogBookEntryAsync(
                    string.Format(_localizer["GuardServiceStart"], person.FirstName, person.LastName),
                    e.StartTime);
        }

        foreach (var id in e.SupervisorPersonIds)
        {
            var person = persons.FirstOrDefault(x => x.Id == id);
            if (person != null)
                await _logBookRepository.InsertLogBookEntryAsync(
                    string.Format(_localizer["SupervisorServiceStart"], person.FirstName, person.LastName),
                    e.StartTime);
        }
    }

    private async Task HandleSentryFinishedAsync(SentryFinishedEvent e)
    {
        var allIds = e.UnfinishedGuardPersonIds.Concat(e.UnfinishedSupervisorPersonIds).Distinct().ToList();
        var persons = await _personRepository.PersonsAsync(allIds);

        foreach (var id in e.UnfinishedGuardPersonIds)
        {
            var person = persons.FirstOrDefault(x => x.Id == id);
            if (person != null)
                await _logBookRepository.InsertLogBookEntryAsync(
                    string.Format(_localizer["GuardServiceFinish"], person.FirstName, person.LastName),
                    e.FinishTime);
        }

        foreach (var id in e.UnfinishedSupervisorPersonIds)
        {
            var person = persons.FirstOrDefault(x => x.Id == id);
            if (person != null)
                await _logBookRepository.InsertLogBookEntryAsync(
                    string.Format(_localizer["SupervisorServiceFinish"], person.FirstName, person.LastName),
                    e.FinishTime);
        }

        await _logBookRepository.InsertLogBookEntryAsync(_localizer["SentryFinished"], e.FinishTime);
    }

    private Task HandleGuardServiceStartedAsync(GuardServiceStartedEvent e)
        => HandleServiceEventAsync(e.PersonId, e.StartTime, "GuardServiceStart");

    private Task HandleGuardServiceEndedAsync(GuardServiceEndedEvent e)
        => HandleServiceEventAsync(e.PersonId, e.EndTime, "GuardServiceFinish");

    private Task HandleSupervisorServiceStartedAsync(SupervisorServiceStartedEvent e)
        => HandleServiceEventAsync(e.PersonId, e.StartTime, "SupervisorServiceStart");

    private Task HandleSupervisorServiceEndedAsync(SupervisorServiceEndedEvent e)
        => HandleServiceEventAsync(e.PersonId, e.EndTime, "SupervisorServiceFinish");

    private async Task HandleServiceEventAsync(string personId, DateTimeOffset time, string localizationKey)
    {
        var persons = await _personRepository.PersonsAsync(new[] { personId });
        var person = persons.FirstOrDefault();
        if (person != null)
            await _logBookRepository.InsertLogBookEntryAsync(
                string.Format(_localizer[localizationKey], person.FirstName, person.LastName),
                time);
    }
}
