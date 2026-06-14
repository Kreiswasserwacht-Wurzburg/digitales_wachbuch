using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories;

public interface IStationRepository
{
    Task<IList<Station>> AllStationsAsync();
    Task<Station> GetStationAsync();
}
