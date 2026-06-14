using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using OrganisationEntity = DigitalGuardBook.Data.Entities.Organisation;
using PersonEntity = DigitalGuardBook.Data.Entities.Person;

namespace DigitalGuardBook.Modules.Organisation
{
    public class OrganisationRepository : IOrganisationRepository
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
                .Lookup<OrganisationEntity, PersonEntity, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.TechnicalLeadIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.TechnicalLeads
                )
                .Lookup<OrganisationComposed, PersonEntity, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.MemberIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.Members
                )
                .Lookup<OrganisationComposed, OrganisationEntity, OrganisationComposed>(
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
                .Lookup<OrganisationEntity, PersonEntity, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.TechnicalLeadIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.TechnicalLeads
                )
                .Lookup<OrganisationComposed, PersonEntity, OrganisationComposed>(
                    _dataContext.Persons,
                    organisation => organisation.MemberIds,
                    person => person.Id,
                    organisationComposed => organisationComposed.Members
                )
                .Lookup<OrganisationComposed, OrganisationEntity, OrganisationComposed>(
                    _dataContext.Organisations,
                    organisation => organisation.SubOrganisationIds,
                    organisation => organisation.Id,
                    organisationComposed => organisationComposed.SubOrganisations
                ).FirstOrDefaultAsync();
        }
    }
}
