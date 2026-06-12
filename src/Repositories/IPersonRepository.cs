using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories;

public interface IPersonRepository
{
    Task<IList<PersonComposed>> AllPersonsAsync();
    Task<IList<PersonComposed>> PersonsAsync(IList<string> ids);
}
