using Microsoft.EntityFrameworkCore;
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

        public async Task<Data.Station> GetStationAsync()
        {
            Data.Station station;

            if (!await _commonDataContext.Stations.AnyAsync()) // Wenn es keine Einträge gibt, nehmen wir einfach das statisch kodierte
            {
                station = new Data.Station
                {
                    Name = "WRST Würzburg",
                    PhoneNumber = "0931 800080",
                    Street = "Mergentheimer Straße 9a",
                    ZipCode = "97082",
                    City = "Würzburg"
                };
            }
            else
            {
                station = await _commonDataContext.Stations.FirstOrDefaultAsync();
            }

            return station;
        }
    }
}
