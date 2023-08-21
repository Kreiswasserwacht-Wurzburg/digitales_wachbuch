using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class SentryStartType : InputObjectGraphType
    {
        public SentryStartType()
        {
            Field<NonNullGraphType<DateTimeOffsetGraphType>>("start");
            Field<DateTimeOffsetGraphType>("registration");
            Field<NonNullGraphType<OrganisationInputType>>("organisation");
            Field<ListGraphType<GuardServiceInputType>>("supervisors");
            Field<ListGraphType<GuardServiceInputType>>("guards");
        }
    }
}   