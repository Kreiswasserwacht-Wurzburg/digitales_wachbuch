
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class OrganisationInputType : InputObjectGraphType
    {
        public OrganisationInputType()
        {
            Field<StringGraphType>("id");
            Field<StringGraphType>("name");
            Field<UShortGraphType>("number");
            Field<ListGraphType<PersonInputType>>("technicalLeads");
            Field<ListGraphType<PersonInputType>>("members");
            Field<ListGraphType<OrganisationInputType>>("subOrganisations");
        }
    }
}