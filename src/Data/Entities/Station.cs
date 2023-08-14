using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalGuardBook.Data.Entities
{
    public class Address
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }

    public class Station
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public Address Address { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string OrganisationId { get; set; }
    }
}