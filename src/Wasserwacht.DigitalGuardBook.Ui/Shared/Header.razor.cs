using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;

namespace Wasserwacht.DigitalGuardBook.Ui.Shared
{
    public partial class Header
    {
        [Inject]
        protected StationService StationService { get; set; }

        private StationModel station;

        protected override async Task OnInitializedAsync()
        {
            station = await StationService.GetStationAsync();
        }
    }
}
