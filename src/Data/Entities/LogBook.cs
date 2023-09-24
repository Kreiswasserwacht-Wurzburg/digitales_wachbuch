using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalGuardBook.Data.Entities
{
    public class LogBookEntry
    {

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTimeOffset Time { get; set; }

        public string Author { get; set; }

        public string Message { get; set; }

        public string Discriminator { get; set; }

        public DateTimeOffset Created { get; set; }
        public string CreatedBy { get; set; }

        public LogBookEntry()
        {
            this.Discriminator = GetDiscriminator();
            this.Created = DateTimeOffset.Now;
            this.CreatedBy = "System";
        }

        protected virtual string GetDiscriminator()
        {
            return nameof(LogBookEntry);
        }
    }

    public class LogBook
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string StationId { get; set; }
        public int Year { get; set; }

        public IList<LogBookEntry> Entries { get; set; }
    }
}