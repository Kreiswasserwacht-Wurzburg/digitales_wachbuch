using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Data;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Services
{
    public class PersonService : BaseService
    {
        private readonly UserManager<Person> _userManager;

        public PersonService(CommonDataContext commonDataContext, UserManager<Person> userManager) : base(commonDataContext)
        {
            _userManager = userManager;
        }

        public async Task<Models.PersonModel> GetPersonAsync(Guid id)
        {
            Data.Person dbPerson = await _commonDataContext.Persons.Include(x => x.Organisations).FirstOrDefaultAsync(x => x.Id == id);

            return new Models.PersonModel(dbPerson);
        }

        public async Task<Models.PersonLoginModel> GetLoginForIdAsync(Guid id)
        {
            Data.Person dbPerson = await _commonDataContext.Persons.FirstOrDefaultAsync(x => x.Id == id);

            return new Models.PersonLoginModel(dbPerson);
        }

        public async Task<List<Models.PersonModel>> GetPersonsByOrganisationAsync(Guid organisationId)
        {
            return await _commonDataContext.Persons
                .Where(x => x.Organisations.Any(y => y.Id == organisationId))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new Models.PersonModel(x))
                .ToListAsync();
        }

        public async Task<Models.PersonModel> UpdaterOrInsertAsync(Models.PersonModel model)
        {
            Data.Person dbPerson = await _commonDataContext.Persons.Include(x => x.Organisations).FirstOrDefaultAsync(x => x.Id == model.Id);

            bool isNew = false;

            if (dbPerson == null)
            {
                isNew = true;
                dbPerson = new Person();
            }

            dbPerson.Id = model.Id;
            dbPerson.FirstName = model.FirstName.Trim();
            dbPerson.MidName = string.IsNullOrEmpty(model.MidName?.Trim()) ? null : model.MidName?.Trim();
            dbPerson.LastName = model.LastName.Trim();
            dbPerson.UserName = $"{model.FirstName.Trim()}.{model.LastName}".ToLower();
            dbPerson.LockoutEnd = DateTimeOffset.MaxValue;

            if (isNew)
            {
                var result = await _userManager.CreateAsync(dbPerson);
            }

            dbPerson = await _commonDataContext.Persons.Include(x => x.Organisations).FirstOrDefaultAsync(x => x.Id == model.Id);

            List<Organisation> currentOrganisations = dbPerson.Organisations?.ToList() ?? new List<Organisation>();
            List<Organisation> newOrganisations = model.Organisations.Select(x => _commonDataContext.Organisations.FirstOrDefault(y => y.Id == x.Id)).ToList();

            var deleted = currentOrganisations.Except(newOrganisations);
            var added = newOrganisations.Except(currentOrganisations);

            foreach (var orgaDeleted in deleted)
            {
                dbPerson.Organisations.Remove(orgaDeleted);
            }

            foreach (var orgaAdded in added)
            {
                dbPerson.Organisations.Add(orgaAdded);
            }

            await _commonDataContext.SaveChangesAsync();

            return model;
        }

        public async Task<Models.PersonLoginModel> UpdateLoginForPerson(Models.PersonLoginModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id.ToString());

            if (user == null)
            {
                throw new KeyNotFoundException();
            }

            if (user.Email != model.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, model.Email.Trim());
                var res = await _userManager.ChangeEmailAsync(user, model.Email.Trim(), token);
            }

            if (!model.HasAccount)
            {
                var res = await _userManager.AddPasswordAsync(user, model.Password.Trim());
                if (res.Succeeded)
                {
                    model.HasAccount = true;

                    var dbUser = await _commonDataContext.Users.FirstAsync(x => x.Id == user.Id);
                    dbUser.UserName = dbUser.Email;
                    dbUser.NormalizedUserName = dbUser.NormalizedEmail;
                    await _commonDataContext.SaveChangesAsync();
                }
            }

            return model;
        }
    }
}
