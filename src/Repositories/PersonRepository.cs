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

        public async Task<List<PersonComposed>> AllPersonsAsync()
        {
            return await _dataContext.Persons
                .Aggregate().Lookup<Person, Organisation, PersonComposed>(
                    _dataContext.Organisations,
                    person => person.OrganisationIds,
                    organisation => organisation.Id,
                    personComposed => personComposed.Organisations
                ).ToListAsync();
        }
    }
}