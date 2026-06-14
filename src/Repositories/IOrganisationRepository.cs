using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories;

public interface IOrganisationRepository
{
    Task<IList<OrganisationComposed>> AllOrganisationsAsync();
    Task<OrganisationComposed> OrganisationAsync(string id);
}
