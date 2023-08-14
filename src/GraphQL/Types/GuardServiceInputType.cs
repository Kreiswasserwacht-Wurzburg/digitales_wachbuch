using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class GuardServiceInputType : InputObjectGraphType
    {
        public GuardServiceInputType()
        {
            Field<NonNullGraphType<DateTimeOffsetGraphType>>("start");
            Field<DateTimeOffsetGraphType>("end");
            Field<NonNullGraphType<PersonInputType>>("guard");
        }
    }
}