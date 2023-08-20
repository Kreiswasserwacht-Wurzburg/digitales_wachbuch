using System.ComponentModel.DataAnnotations.Schema;
using DigitalGuardBook.GraphQL.Types;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalGuardBook.Data.Entities
{
    public class GuardServiceComposed : GuardService
    {
        [BsonIgnore]
        public DateTimeOffset Start { get; set; }
        [BsonIgnore]
        public DateTimeOffset? End { get; set; }
        [BsonIgnore]
        public Person Guard { get; set; }
    }

    public class SentryComposed : Sentry
    {
        [BsonIgnore]
        public Organisation Organisation { get; set; }
        [BsonIgnore]
        public IList<GuardServiceComposed> Supervisors { get; set; } = new List<GuardServiceComposed>();
        [BsonIgnore]
        public IList<GuardServiceComposed> Guards { get; set; } = new List<GuardServiceComposed>();
    }
}