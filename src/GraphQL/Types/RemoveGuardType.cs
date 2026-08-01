using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class RemoveGuardType : InputObjectGraphType
    {
        public RemoveGuardType()
        {
            Field<NonNullGraphType<StringGraphType>>("sentryId");
            Field<NonNullGraphType<StringGraphType>>("personId");
            Field<NonNullGraphType<DateTimeOffsetGraphType>>("end");
        }
    }
}
