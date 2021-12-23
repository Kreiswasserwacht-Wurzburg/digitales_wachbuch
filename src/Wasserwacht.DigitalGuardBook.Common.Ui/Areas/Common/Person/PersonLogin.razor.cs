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
    public partial class PersonLogin
    {
        [Parameter]
        public Guid Id { get; set; }

        protected EditContext editContext { get; set; }

        [Inject]
        protected PersonService PersonService { get; set; }

        private PersonLoginModel login = new PersonLoginModel();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            editContext = new EditContext(this.login);
            editContext.SetFieldCssClassProvider(new BootstrapClassProvider());
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            var dbPerson = await this.PersonService.GetLoginForIdAsync(Id);

            SetValues(dbPerson);
        }

        private void SetValues(PersonLoginModel dbPerson)
        {
            login.Id = dbPerson.Id;
            login.FirstName = dbPerson.FirstName;
            login.MidName = dbPerson.MidName;
            login.LastName = dbPerson.LastName;
            login.HasAccount = dbPerson.HasAccount;
            login.Password = dbPerson.Password;
            login.Email = dbPerson.Email;
        }

        private async Task HandleValidSubmit(EditContext editContext)
        {
            PersonLoginModel dbPerson = await PersonService.UpdateLoginForPerson(login);

            SetValues(dbPerson);

            editContext.Validate();
        }

        private async Task OnResetClicked(Guid id)
        {

        }
    }
}
