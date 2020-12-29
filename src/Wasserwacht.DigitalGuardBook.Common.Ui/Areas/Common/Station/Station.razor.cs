using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Station
{
    public class StationModel
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Street { get; set; }

        [MaxLength(20)]
        public string ZipCode { get; set; }

        [MaxLength(100)]
        public string City { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }

    public partial class Station
    {
        private DigitalGuardBook.Common.Data.Station station = new DigitalGuardBook.Common.Data.Station();
        private EditContext editContext;

        private async Task HandleValidSubmit(EditContext editContext)
        {
            editContext.NotifyValidationStateChanged();
        }

        protected async override Task OnInitializedAsync()
        {
            editContext = new EditContext(station);
            editContext.SetFieldCssClassProvider(new BootstrapClassProvider());

            station = await StationService.GetStationAsync();
        }
    }
}
