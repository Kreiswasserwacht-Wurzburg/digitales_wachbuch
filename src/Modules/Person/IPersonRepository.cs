using DigitalGuardBook.Data.Entities;
using PersonEntity = DigitalGuardBook.Data.Entities.Person;

namespace DigitalGuardBook.Modules.Person;

public interface IPersonRepository
{
    Task<IList<PersonComposed>> AllPersonsAsync();
    Task<IList<PersonComposed>> PersonsAsync(IList<string> ids);
}
