using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Logic.Models;
using Wasserwacht.DigitalGuardBook.Common.Logic.Services;

namespace Wasserwacht.DigitalGuardBook.Common.Ui.Areas.Common.Person
{
    public partial class PersonEdit
    {
        [Parameter]
        public Guid Id { get; set; }

        protected EditContext editContext { get; set; }

        [Inject]
        protected OrganisationService OrganisationService { get; set; }

        [Inject]
        protected PersonService PersonService { get; set; }

        private PersonModel person = new PersonModel();

        private List<OrganisationModel> organisations = new List<OrganisationModel>();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            editContext = new EditContext(this.person);
            editContext.SetFieldCssClassProvider(new BootstrapClassProvider());
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var personTask = this.PersonService.GetPersonAsync(Id);
            var organisationsTask = this.OrganisationService.GetOrganisationsAsync();

            var dbPerson = await personTask;
            SetValues(dbPerson);

            organisations = await organisationsTask;

            editContext.Validate();
        }

        private void SetValues(PersonModel dbPerson)
        {
            person.Id = dbPerson.Id;
            person.FirstName = dbPerson.FirstName;
            person.MidName = dbPerson.MidName;
            person.LastName = dbPerson.LastName;
            person.Gender = dbPerson.Gender;
            person.Organisations = dbPerson.Organisations;
        }

        private async Task HandleValidSubmit(EditContext editContext)
        {
            PersonModel dbPerson = await PersonService.UpdaterOrInsertAsync(person);

            SetValues(dbPerson);

            editContext.Validate();
        }

        private List<KeyValuePair<Data.Gender, string>> GetGenders()
        {
            var res = new List<KeyValuePair<Data.Gender, string>>();

            foreach (int i in Enum.GetValues(typeof(Data.Gender)))
            {
                var name = Enum.GetName(typeof(Data.Gender), i);
                res.Add(new KeyValuePair<Data.Gender, string>((Data.Gender)i, name));
            }

            return res;
        }

        private void OnChange(IEnumerable<Guid> value)
        {
            person.Organisations = organisations.Where(x => value.Contains(x.Id)).ToList();

            editContext.Validate();
        }
    }
}
