using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using PersonEntity = DigitalGuardBook.Data.Entities.Person;
using OrganisationEntity = DigitalGuardBook.Data.Entities.Organisation;

namespace DigitalGuardBook.Modules.Person
{
    public class PersonRepository : IPersonRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;

        public PersonRepository(DigitalGuardBookDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<PersonComposed>> AllPersonsAsync()
        {
            return await _dataContext.Persons
                .Aggregate()
                .Lookup<PersonEntity, OrganisationEntity, PersonComposed>(
                    _dataContext.Organisations,
                    person => person.OrganisationIds,
                    organisation => organisation.Id,
                    personComposed => personComposed.Organisations
                ).ToListAsync();
        }

        public async Task<IList<PersonComposed>> PersonsAsync(IList<string> ids)
        {
            return await _dataContext.Persons
                .Aggregate()
                .Match(person => ids.Contains(person.Id))
                .Lookup<PersonEntity, OrganisationEntity, PersonComposed>(
                        _dataContext.Organisations,
                        person => person.OrganisationIds,
                        organisation => organisation.Id,
                        personComposed => personComposed.Organisations
                ).ToListAsync();
        }
    }
}
