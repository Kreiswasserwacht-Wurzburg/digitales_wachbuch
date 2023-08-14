using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DigitalGuardBook.Data.Entities
{
    public class GuardService
    {
        public GuardService() { }

        public GuardService(GuardService service)
        {
            this.PersonId = service.PersonId;
            this.Start = service.Start;
            this.End = service.End;
        }

        [BsonRepresentation(BsonType.ObjectId)]
        public string PersonId { get; set; }
        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
    }

    public class Sentry
    {
        public Sentry() { }

        public Sentry(Sentry sentry)
        {
            this.Id = sentry.Id;
            this.OrganisationId = sentry.OrganisationId;
            this.Start = sentry.Start;
            this.End = sentry.End;
            this.Registration = sentry.Registration;
            this.SupervisorServices = sentry.SupervisorServices;
            this.GuardServices = sentry.GuardServices;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string OrganisationId { get; set; }

        public DateTimeOffset Start { get; set; }
        public DateTimeOffset? End { get; set; }
        public DateTimeOffset? Registration { get; set; }

        public IList<GuardService> SupervisorServices { get; set; } = new List<GuardService>();

        public IList<GuardService> GuardServices { get; set; } = new List<GuardService>();
    }
}