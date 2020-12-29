using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Ui
{
    public partial class StationInfo
    {
        private Data.Station station;

        protected override async Task OnInitializedAsync()
        {
            station = await StationService.GetStationAsync();
        }
    }
}
