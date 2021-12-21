using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;

namespace Wasserwacht.DigitalGuardBook.Common.Ui
{
    public partial class StationInfo
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
