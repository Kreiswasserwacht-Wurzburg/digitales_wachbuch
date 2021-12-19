using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;


namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Organisation
{
    public partial class OrganisationEdit
    {
        [Parameter]
        public Guid Id { get; set; }

        private bool isAdd = false;

        protected EditContext editContext { get; set; }

        [Inject]
        protected OrganisationService OrganisationService { get; set; }

        private OrganisationModel organisation = new OrganisationModel();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            editContext = new EditContext(this.organisation);
            editContext.SetFieldCssClassProvider(new BootstrapClassProvider());
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var dbOrganisation = await this.OrganisationService.GetOrganisationAsync(Id);

            SetValues(dbOrganisation);

            editContext.Validate();
        }

        private void SetValues(OrganisationModel model)
        {
            organisation.Id = model.Id;
            organisation.Name = model.Name;
            organisation.Number = model.Number;
        }

        private async Task HandleValidSubmit(EditContext editContext)
        {
            OrganisationModel dbOrganisation = await OrganisationService.UpdaterOrInsertAsync(organisation);

            SetValues(dbOrganisation);

            editContext.Validate();
        }
    }
}
