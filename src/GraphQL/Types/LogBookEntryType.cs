using DigitalGuardBook.Data.Entities;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class LogBookEntryType : ObjectGraphType<LogBookEntry>
    {
        public LogBookEntryType()
        {
            Field(x => x.Id);
            Field(x => x.Author);
            Field(x => x.Time);
            Field(x => x.Message);
        }
    }
}