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
                var sentry = context.GetArgument<Sentry>("sentry");
                return await sentryRepository.StartSentryAsync(sentry);
            });
        }
    }
}   