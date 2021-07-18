using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Data;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Services
{
    public class StationService : BaseService
    {
        public StationService(CommonDataContext commonDataContext) : base(commonDataContext)
        {
        }

        public async Task<Models.StationModel> GetStationAsync()
        {
            Data.Station dbStation;

            if (!await _commonDataContext.Stations.AnyAsync()) // Wenn es keine Einträge gibt, nehmen wir einfach das statisch kodierte
            {
                dbStation = null;
            }
            else
            {
                dbStation = await _commonDataContext.Stations.FirstOrDefaultAsync();
            }

            return new Models.StationModel
            {
                Id = dbStation?.Id,
                City = dbStation?.City,
                Name = dbStation?.Name,
                PhoneNumber = dbStation?.PhoneNumber,
                Street = dbStation?.Street,
                ZipCode = dbStation?.ZipCode,
            };
        }

        public async Task<Models.StationModel> UpdateStation(Models.StationModel model)
        {
            bool isNew = !model.Id.HasValue || model.Id == Guid.Empty;

            Data.Station dbStation = await _commonDataContext.Stations.FirstOrDefaultAsync() ?? new Station();

            dbStation.Id = isNew ? Guid.NewGuid() : model.Id.Value;
            dbStation.City = model.City;
            dbStation.Name = model.Name;
            dbStation.PhoneNumber = model.PhoneNumber;
            dbStation.Street = model.Street;
            dbStation.ZipCode = model.ZipCode;

            if (isNew)
            {
                dbStation.Created = DateTime.UtcNow;
                await _commonDataContext.AddAsync(dbStation);
            }

            await _commonDataContext.SaveChangesAsync();

            model.Id = dbStation.Id;
            return model;
        }
    }
}
