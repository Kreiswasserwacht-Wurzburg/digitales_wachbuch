using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Data
{
    public class DigitalGuardBookDataContext
    {
        public IMongoCollection<Person> Persons => _database.GetCollection<Person>("person");
        public IMongoCollection<Organisation> Organisations => _database.GetCollection<Organisation>("organisation");

        private readonly IMongoDatabase _database;

        public DigitalGuardBookDataContext()
        {
            var settings = MongoClientSettings.FromConnectionString("mongodb://localhost:27017");
            settings.ClusterConfigurator = builder => builder.Subscribe<CommandStartedEvent>(started =>
            {
                Console.WriteLine("MongoDB Command: " + started.Command);
            });

            var client = new MongoClient(settings);
            _database = client.GetDatabase("DigitalGuardBook");

            if (!client.ListDatabaseNames().ToList().Contains("DigitalGuardBook"))
            {
                Task.Run(async () => await SeedData());
            };
        }

        private async Task SeedData()
        {
            var person1 = new Person
            {
                FirstName = "Angelika",
                LastName = "Bayer",
                Gender = Gender.Female
            };

            var person2 = new Person
            {
                FirstName = "Kevin",
                LastName = "Moench",
                Gender = Gender.Male
            };

            await Persons.InsertOneAsync(person1);
            await Persons.InsertOneAsync(person2);

            var organisation1 = new Organisation
            {
                Name = "KWW Kleinstausbad",
                Number = 1,
                TechnicalLeadIds = new List<string> { person1.Id }
            };
            await Organisations.InsertOneAsync(organisation1);

            var organisation2 = new Organisation
            {
                Name = "OG Kleinstausbad",
                Number = 2,
                TechnicalLeadIds = new List<string> { person2.Id },
                ParentOrganisationId = organisation1.Id
            };

            await Organisations.InsertOneAsync(organisation2);

            var updateOrganisations1 = Builders<Organisation>.Update
                .Push(organisation => organisation.SubOrganisationIds, organisation2.Id);

            await Organisations.UpdateOneAsync(x => x.Id == organisation1.Id, updateOrganisations1);

            var updatePerson1 = Builders<Person>.Update
                .PushEach(person => person.OrganisationIds, new List<string> { organisation1.Id, organisation2.Id });
            await Persons.UpdateOneAsync(x => x.Id == person1.Id, updatePerson1);

            var updatePerson2 = Builders<Person>.Update
                .Push(person => person.OrganisationIds, organisation2.Id);
            await Persons.UpdateOneAsync(x => x.Id == person2.Id, updatePerson2);

            var updateOrganisationMembers1 = Builders<Organisation>.Update
                .Push(organisation => organisation.MemberIds, person1.Id);
            await Organisations.UpdateOneAsync(x => x.Id == organisation1.Id, updateOrganisationMembers1);

            var updateOrganisationMembers2 = Builders<Organisation>.Update
                .PushEach(organisation => organisation.MemberIds, new List<string> { person1.Id, person2.Id });
            await Organisations.UpdateOneAsync(x => x.Id == organisation2.Id, updateOrganisationMembers2);
        }
    }
}
