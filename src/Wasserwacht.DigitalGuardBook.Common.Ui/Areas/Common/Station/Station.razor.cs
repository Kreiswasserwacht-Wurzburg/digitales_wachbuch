using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Diagnostics;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;

namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Station
{
    public partial class Station
    {

        protected EditContext editContext { get; set; }

        [Inject]
        protected StationService StationService { get; set; }

        private StationModel station = new StationModel();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            editContext = new EditContext(this.station);
            editContext.SetFieldCssClassProvider(new BootstrapClassProvider());

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var dbStation = await this.StationService.GetStationAsync();

            SetValues(dbStation);

            editContext.Validate();
        }

        private void SetValues(StationModel model)
        {
            station.Id = model.Id;
            station.City = model.City;
            station.Name = model.Name;
            station.PhoneNumber = model.PhoneNumber;
            station.Street = model.Street;
            station.ZipCode = model.ZipCode;
        }

        private async Task HandleValidSubmit(EditContext editContext)
        {
            var dbStation = await StationService.UpdateStation(station);

            SetValues(dbStation);

            editContext.Validate();
        }
    }
}
