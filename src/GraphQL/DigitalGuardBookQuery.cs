using DigitalGuardBook.GraphQL.Types;
using DigitalGuardBook.Modules.Sentry;
using DigitalGuardBook.Modules.Sentry.Queries;
using DigitalGuardBook.Modules.Person;
using DigitalGuardBook.Modules.Organisation;
using DigitalGuardBook.Modules.Station;
using DigitalGuardBook.Modules.LogBook;
using DigitalGuardBook.Modules.LogBook.Queries;
using GraphQL;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookQuery : ObjectGraphType
    {
        public DigitalGuardBookQuery(IPersonRepository personRepository, IOrganisationRepository organisationRepository, IStationRepository stationRepository, GetActiveSentryQueryHandler getActiveSentryHandler, GetLogBookEntriesQueryHandler getLogBookEntriesHandler)
        {
            Field<ListGraphType<PersonType>>("persons").ResolveAsync(async context => await personRepository.AllPersonsAsync());
            Field<ListGraphType<OrganisationType>>("organisations").ResolveAsync(async context => await organisationRepository.AllOrganisationsAsync());
            Field<ListGraphType<StationType>>("stations").ResolveAsync(async context => await stationRepository.AllStationsAsync());
            Field<StationType>("station").ResolveAsync(async context => await stationRepository.GetStationAsync());
            Field<SentryType>("activeSentry").ResolveAsync(async context => await getActiveSentryHandler.HandleAsync(new GetActiveSentryQuery()));
            Field<ListGraphType<LogBookEntryType>>("logBookEntries")
            .Argument<string>("from", nullable: false)
            .Argument<string>("to", nullable: true)
            .ResolveAsync(async context =>
            {
                var from = DateTimeOffset.Parse(context.GetArgument<string>("from"));
                var toStr = context.GetArgument("to", string.Empty);
                var to = string.IsNullOrEmpty(toStr) ? (DateTimeOffset?)null : DateTimeOffset.Parse(toStr);

                return await getLogBookEntriesHandler.HandleAsync(new GetLogBookEntriesQuery(from, to));
            }
            );
        }
    }
}