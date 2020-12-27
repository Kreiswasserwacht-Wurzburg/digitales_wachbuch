using System;
using System.Linq;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Repositories
{
    public class StationRepository
    {
        public async Task<Station> GetStationAsync()
        {
            return new Station
            {
                Name = "WRST Würzburg",
                PhoneNumber = "0931 800080",
                Street = "Mergentheimer Straße 9a",
                ZipCode = "97082",
                City = "Würzburg"
            };
        }
    }
}
