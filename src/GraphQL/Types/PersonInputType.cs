using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class PersonInputType : InputObjectGraphType
    {
        public PersonInputType()
        {
            Field<StringGraphType>("id");
            Field<StringGraphType>("firstName");
            Field<StringGraphType>("lastName");
            Field<GenderEnumType>("gender");
        }
    }
}