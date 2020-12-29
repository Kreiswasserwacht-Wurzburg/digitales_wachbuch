using System;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Station
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
