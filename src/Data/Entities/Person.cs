using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalGuardBook.Data.Entities
{
    public class Person
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string> OrganisationIds { get; set; } = new List<string>();
    }
}