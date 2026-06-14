using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Modules.Person;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class GuardServiceType : ObjectGraphType<GuardService>
    {
        public GuardServiceType(IPersonRepository personRepository)
        {
            Field(x => x.Start);
            Field(x => x.End, nullable: true);
            Field<PersonType>("guard").ResolveAsync(async context => (await personRepository.PersonsAsync(new List<string> { context.Source.PersonId })).First());
        }
    }
}