using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class SentryFinishType : InputObjectGraphType
    {
        public SentryFinishType()
        {
            Field<NonNullGraphType<StringGraphType>>("id");
            Field<DateTimeOffsetGraphType>("finish");
        }
    }
}   