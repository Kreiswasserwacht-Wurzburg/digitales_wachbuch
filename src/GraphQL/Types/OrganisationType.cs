using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Modules.Person;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class OrganisationType : ObjectGraphType<OrganisationComposed>
    {
        public OrganisationType(IPersonRepository personRepository)
        {
            Field(organisation => organisation.Id);
            Field(organisation => organisation.Name);
            Field(organisation => organisation.Number);
            Field<ListGraphType<PersonType>>("technicalLeads")
            .ResolveAsync(async context => await personRepository.PersonsAsync(context.Source.TechnicalLeadIds));
            Field<ListGraphType<PersonType>>("members")
            .ResolveAsync(async context => await personRepository.PersonsAsync(context.Source.MemberIds));
            Field<ListGraphType<OrganisationType>>("subOrganisations")
            .Resolve(context => context.Source.SubOrganisations);
        }
    }
}