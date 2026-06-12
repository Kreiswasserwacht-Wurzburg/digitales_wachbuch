using DigitalGuardBook.Events;
using DigitalGuardBook.Infrastructure;
using DigitalGuardBook.Repositories;
using Microsoft.Extensions.Localization;

namespace DigitalGuardBook.EventHandlers;

public sealed class LogBookEventHandler
{
    private readonly LogBookRepository _logBookRepository;
    private readonly PersonRepository _personRepository;
    private readonly IStringLocalizer<LogBookEventHandler> _localizer;

    public LogBookEventHandler(
        InProcessEventPublisher publisher,
        LogBookRepository logBookRepository,
        PersonRepository personRepository,
        IStringLocalizer<LogBookEventHandler> localizer)
    {
        _logBookRepository = logBookRepository;
        _personRepository = personRepository;
        _localizer = localizer;

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

    private async Task HandleGuardServiceStartedAsync(GuardServiceStartedEvent e)
    {
        var persons = await _personRepository.PersonsAsync(new[] { e.PersonId });
        var person = persons.FirstOrDefault();
        if (person != null)
            await _logBookRepository.InsertLogBookEntryAsync(
                string.Format(_localizer["GuardServiceStart"], person.FirstName, person.LastName),
                e.StartTime);
    }

    private async Task HandleGuardServiceEndedAsync(GuardServiceEndedEvent e)
    {
        var persons = await _personRepository.PersonsAsync(new[] { e.PersonId });
        var person = persons.FirstOrDefault();
        if (person != null)
            await _logBookRepository.InsertLogBookEntryAsync(
                string.Format(_localizer["GuardServiceFinish"], person.FirstName, person.LastName),
                e.EndTime);
    }

    private async Task HandleSupervisorServiceStartedAsync(SupervisorServiceStartedEvent e)
    {
        var persons = await _personRepository.PersonsAsync(new[] { e.PersonId });
        var person = persons.FirstOrDefault();
        if (person != null)
            await _logBookRepository.InsertLogBookEntryAsync(
                string.Format(_localizer["SupervisorServiceStart"], person.FirstName, person.LastName),
                e.StartTime);
    }

    private async Task HandleSupervisorServiceEndedAsync(SupervisorServiceEndedEvent e)
    {
        var persons = await _personRepository.PersonsAsync(new[] { e.PersonId });
        var person = persons.FirstOrDefault();
        if (person != null)
            await _logBookRepository.InsertLogBookEntryAsync(
                string.Format(_localizer["SupervisorServiceFinish"], person.FirstName, person.LastName),
                e.EndTime);
    }
}
