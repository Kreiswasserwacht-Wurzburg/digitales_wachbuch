using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;

namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Organisation
{
    public partial class OrganisationList
    {
        [Inject]
        protected NavigationManager _NavigationManager { get; set; }

        [Inject]
        protected OrganisationService OrganisationService { get; set; }

        private List<OrganisationModel> organisations = new List<OrganisationModel>();

        private void OnAddClicked()
        {
            _NavigationManager.NavigateTo($"/Administration/Organisation/{Guid.NewGuid()}");
        }

        private void OnEditClicked(Guid id)
        {
            _NavigationManager.NavigateTo($"/Administration/Organisation/{id}");
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            organisations = await OrganisationService.GetOrganisationsAsync();
        }
    }
}
