using DigitalGuardBook.Data.Entities;
using OrganisationEntity = DigitalGuardBook.Data.Entities.Organisation;

namespace DigitalGuardBook.Modules.Organisation;

public interface IOrganisationRepository
{
    Task<IList<OrganisationComposed>> AllOrganisationsAsync();
    Task<OrganisationComposed> OrganisationAsync(string id);
}
