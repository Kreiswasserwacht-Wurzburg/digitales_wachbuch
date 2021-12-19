using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;

namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Person
{
    public partial class PersonList
    {
        [Inject]
        protected NavigationManager _NavigationManager { get; set; }

        [Parameter]
        public List<PersonModel> Persons { get; set; }

        private void OnAddClicked()
        {
            _NavigationManager.NavigateTo($"/Administration/Person/{Guid.NewGuid()}");
        }

        private void OnEditClicked(Guid id)
        {
            _NavigationManager.NavigateTo($"/Administration/Person/{id}");
        }
    }
}
