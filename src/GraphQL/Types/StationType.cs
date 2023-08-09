using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Repositories;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class StationType : ObjectGraphType<Station>
    {
        public StationType(OrganisationRepository organisationRepository)
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field<AddressType>("address").Resolve(context => context.Source.Address);
            Field<OrganisationType>("organisation")
            .ResolveAsync(async context => await organisationRepository.OrganisationAsync(context.Source.OrganisationId));
        }
    }
}