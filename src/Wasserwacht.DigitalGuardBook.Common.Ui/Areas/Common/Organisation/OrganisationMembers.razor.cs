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
    public partial class OrganisationMembers
    {
        [Inject]
        protected PersonService PersonService { get; set; }

        [Parameter]
        public Guid Id { get; set; }

        private List<PersonModel> persons = new List<PersonModel>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            persons = await PersonService.GetPersonsByOrganisationAsync(Id);
        }

    }
}
