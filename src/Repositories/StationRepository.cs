using MongoDB.Driver;
using MongoDB.Driver.Linq;
using DigitalGuardBook.Data;
using DigitalGuardBook.Data.Entities;

namespace DigitalGuardBook.Repositories
{
    public class StationRepository
    {
        private readonly DigitalGuardBookDataContext _dataContext;

        public StationRepository(DigitalGuardBookDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<IList<Station>> AllStationsAsync()
        {
            return await _dataContext.Stations
                .AsQueryable()
                .ToListAsync();
        }

        public async Task<Station> GetStationAsync()
        {
            return await _dataContext.Stations
                .AsQueryable()
                .FirstOrDefaultAsync();
        }
    }
}