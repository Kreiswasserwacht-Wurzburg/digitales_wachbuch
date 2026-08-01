using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class AddGuardType : InputObjectGraphType
    {
        public AddGuardType()
        {
            Field<NonNullGraphType<StringGraphType>>("sentryId");
            Field<NonNullGraphType<DateTimeOffsetGraphType>>("start");
            Field<NonNullGraphType<PersonInputType>>("guard");
        }
    }
}
