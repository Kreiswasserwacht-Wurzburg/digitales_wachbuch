using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories
{
    public class PersonRepository
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
                .Lookup<Person, Organisation, PersonComposed>(
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
                .Lookup<Person, Organisation, PersonComposed>(
                        _dataContext.Organisations,
                        person => person.OrganisationIds,
                        organisation => organisation.Id,
                        personComposed => personComposed.Organisations
                ).ToListAsync();
        }
    }
}