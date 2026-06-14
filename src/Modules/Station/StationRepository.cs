using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;
using StationEntity = DigitalGuardBook.Data.Entities.Station;

namespace DigitalGuardBook.Modules.Station
{
    public class StationRepository : IStationRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;

        public StationRepository(DigitalGuardBookDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<StationEntity>> AllStationsAsync()
        {
            return await _dataContext.Stations
                .AsQueryable()
                .ToListAsync();
        }

        public async Task<StationEntity> GetStationAsync()
        {
            return await _dataContext.Stations
                .AsQueryable()
                .FirstOrDefaultAsync();
        }
    }
}
