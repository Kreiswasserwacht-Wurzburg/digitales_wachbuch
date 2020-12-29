using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Repositories
{
    public class StationRepository
    {
        public async Task<Data.Station> GetStationAsync()
        {
            return new Data.Station
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
