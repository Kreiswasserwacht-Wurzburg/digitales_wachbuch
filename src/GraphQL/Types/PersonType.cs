using DigitalGuardBook.Data.Entities;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class PersonType : ObjectGraphType<PersonComposed>
    {
        public PersonType()
        {
            Field(person => person.Id);
            Field(person => person.FirstName);
            Field(person => person.LastName);
            Field(person => person.Gender);
            Field<ListGraphType<OrganisationType>>("organisations").Resolve(context => context.Source.Organisations);
        }
    }
}