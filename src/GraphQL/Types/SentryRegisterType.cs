using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class SentryRegisterType : InputObjectGraphType
    {
        public SentryRegisterType()
        {
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<NonNullGraphType<DateTimeOffsetGraphType>>("registration");
        }
    }
}   