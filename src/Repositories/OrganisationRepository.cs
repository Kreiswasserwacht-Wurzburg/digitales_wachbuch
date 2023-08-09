using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories
{
    public class OrganisationRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;

        public OrganisationRepository(DigitalGuardBookDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<OrganisationComposed>> AllOrganisationsAsync()
        {
            return await _dataContext.Organisations
                .Aggregate()
                .Lookup<Organisation, Person, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.TechnicalLeadIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.TechnicalLeads
                )
                .Lookup<OrganisationComposed, Person, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.MemberIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.Members
                )
                .Lookup<OrganisationComposed, Organisation, OrganisationComposed>(
                    _dataContext.Organisations,
                    organisation => organisation.SubOrganisationIds,
                    organisation => organisation.Id,
                    organisationComposed => organisationComposed.SubOrganisations
                )
                .ToListAsync();
        }

        public async Task<OrganisationComposed> OrganisationAsync(string id)
        {
            return await _dataContext.Organisations
                .Aggregate()
                .Match(organisation => organisation.Id == id)
                .Lookup<Organisation, Person, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.TechnicalLeadIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.TechnicalLeads
                )
                .Lookup<OrganisationComposed, Person, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.MemberIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.Members
                )
                .Lookup<OrganisationComposed, Organisation, OrganisationComposed>(
                    _dataContext.Organisations,
                    organisation => organisation.SubOrganisationIds,
                    organisation => organisation.Id,
                    organisationComposed => organisationComposed.SubOrganisations
                ).FirstOrDefaultAsync();
        }
    }
}