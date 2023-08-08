using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalGuardBook.Data.Entities
{
    public class Organisation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }
        public ushort Number { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string> TechnicalLeadIds { get; set; } = new List<string>();

        [BsonRepresentation(BsonType.ObjectId)]
        public string? ParentOrganisationId { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public IList<string> SubOrganisationIds { get; set; } = new List<string>();
    }
}