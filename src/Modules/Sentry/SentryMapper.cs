using DigitalGuardBook.Data.Entities;
using SentryEntity = DigitalGuardBook.Data.Entities.Sentry;

namespace DigitalGuardBook.Modules.Sentry;

public static class SentryMapper
{
    public static SentryEntity MapStartSentryInput(SentryComposed input)
    {
        input.OrganisationId = input.Organisation.Id;

        var guardServices = input.Guards.Select(x => new GuardService
        {
            Start = x.Start,
            End = x.End,
            PersonId = x.Guard.Id
        }).ToList();

        var supervisorServices = input.Supervisors.Select(x => new GuardService
        {
            Start = x.Start,
            End = x.End,
            PersonId = x.Guard.Id
        }).ToList();

        guardServices.AddRange(supervisorServices);

        input.GuardServices = guardServices;
        input.SupervisorServices = supervisorServices;

        return input;
    }
}
