using DigitalGuardBook.GraphQL.Types;
using DigitalGuardBook.Repositories;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookQuery : ObjectGraphType
    {
        public DigitalGuardBookQuery(PersonRepository personRepository, OrganisationRepository organisationRepository, StationRepository stationRepository, SentryRepository sentryRepository)
        {
            Field<ListGraphType<PersonType>>("persons").ResolveAsync(async context => await personRepository.AllPersonsAsync());
            Field<ListGraphType<OrganisationType>>("organisations").ResolveAsync(async context => await organisationRepository.AllOrganisationsAsync());
            Field<ListGraphType<StationType>>("stations").ResolveAsync(async context => await stationRepository.AllStationsAsync());
            Field<StationType>("station").ResolveAsync(async context => await stationRepository.GetStationAsync());
            Field<SentryType>("activeSentry").ResolveAsync(async context => await sentryRepository.GetActiveSentry());
        }
    }
}