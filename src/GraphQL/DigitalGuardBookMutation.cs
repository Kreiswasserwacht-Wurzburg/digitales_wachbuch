using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.GraphQL.Types;
using DigitalGuardBook.Repositories;
using DigitalGuardBook.Modules.Sentry;
using GraphQL;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookMutation : ObjectGraphType
    {
        public DigitalGuardBookMutation(ISentryService sentryService)
        {
            Field<SentryType>("startSentry")
            .Argument<NonNullGraphType<SentryStartType>>("sentry")
            .ResolveAsync(async context =>
            {
                var input = context.GetArgument<SentryComposed>("sentry");
                var sentry = SentryMapper.MapStartSentryInput(input);
                return await sentryService.StartSentryAsync(sentry);
            });

            Field<StringGraphType>("finishSentry")
            .Argument<NonNullGraphType<SentryFinishType>>("sentry")
            .ResolveAsync(async context =>
            {
                var sentry = context.GetArgument<Sentry>("sentry");

                await sentryService.FinishSentryAsync(sentry.Id, sentry.End.GetValueOrDefault(DateTimeOffset.Now));

                return sentry.Id;
            });
        }
    }
}