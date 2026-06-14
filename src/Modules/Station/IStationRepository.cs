using DigitalGuardBook.Data.Entities;
using StationEntity = DigitalGuardBook.Data.Entities.Station;

namespace DigitalGuardBook.Modules.Station;

public interface IStationRepository
{
    Task<IList<StationEntity>> AllStationsAsync();
    Task<StationEntity> GetStationAsync();
}
