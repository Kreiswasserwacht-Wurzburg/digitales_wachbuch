using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.GraphQL.Types;
using DigitalGuardBook.Repositories;
using GraphQL;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookMutation : ObjectGraphType
    {
        public DigitalGuardBookMutation(SentryRepository sentryRepository)
        {
            Field<SentryType>("startSentry")
            .Argument<NonNullGraphType<SentryStartType>>("sentry")
            .ResolveAsync(async context =>
            {
                var sentry = context.GetArgument<SentryComposed>("sentry");
                sentry.OrganisationId = sentry.Organisation.Id;
                sentry.GuardServices = sentry.Guards.Select(x=> new GuardService {
                    Start = x.Start,
                    End = x.End,
                    PersonId = x.Guard.Id
                }).ToList();

                sentry.SupervisorServices = sentry.Supervisors.Select(x=> new GuardService {
                    Start = x.Start,
                    End = x.End,
                    PersonId = x.Guard.Id
                }).ToList();
                return await sentryRepository.StartSentryAsync(sentry);
            });
        }
    }
}   