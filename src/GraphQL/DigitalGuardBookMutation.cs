using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.GraphQL.Types;
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

            Field<SentryType>("addGuard")
            .Argument<NonNullGraphType<AddGuardType>>("guard")
            .ResolveAsync(async context =>
            {
                var input = context.GetArgument<dynamic>("guard");
                var guardService = new GuardService
                {
                    Start = input["start"],
                    PersonId = input["guard"]["id"]
                };
                return await sentryService.AddGuardAsync(input["sentryId"], guardService);
            });

            Field<SentryType>("removeGuard")
            .Argument<NonNullGraphType<RemoveGuardType>>("guard")
            .ResolveAsync(async context =>
            {
                var input = context.GetArgument<dynamic>("guard");
                return await sentryService.RemoveGuardAsync(input["sentryId"], input["personId"], input["end"]);
            });
        }
    }
}