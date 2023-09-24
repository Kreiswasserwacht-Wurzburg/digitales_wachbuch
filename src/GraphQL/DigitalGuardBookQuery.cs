using DigitalGuardBook.GraphQL.Types;
using DigitalGuardBook.Repositories;
using GraphQL;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookQuery : ObjectGraphType
    {
        public DigitalGuardBookQuery(PersonRepository personRepository, OrganisationRepository organisationRepository, StationRepository stationRepository, SentryRepository sentryRepository, LogBookRepository logBookRepository)
        {
            Field<ListGraphType<PersonType>>("persons").ResolveAsync(async context => await personRepository.AllPersonsAsync());
            Field<ListGraphType<OrganisationType>>("organisations").ResolveAsync(async context => await organisationRepository.AllOrganisationsAsync());
            Field<ListGraphType<StationType>>("stations").ResolveAsync(async context => await stationRepository.AllStationsAsync());
            Field<StationType>("station").ResolveAsync(async context => await stationRepository.GetStationAsync());
            Field<SentryType>("activeSentry").ResolveAsync(async context => await sentryRepository.GetActiveSentry());
            Field<ListGraphType<LogBookEntryType>>("logBookEntries")
            .Argument<string>("from", nullable: false)
            .Argument<string>("to", nullable: true)
            .ResolveAsync(async context =>
            {
                var from = DateTimeOffset.Parse(context.GetArgument<string>("from"));
                var toStr = context.GetArgument("to", string.Empty);
                var to = string.IsNullOrEmpty(toStr) ? (DateTimeOffset?)null : DateTimeOffset.Parse(toStr);

                return await logBookRepository.GetEntriesForLogBookAsync(from, to);
            }
            );
        }
    }
}