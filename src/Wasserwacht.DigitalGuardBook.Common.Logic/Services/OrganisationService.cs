using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Data;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Services
{
    public class OrganisationService : BaseService
    {
        public OrganisationService(CommonDataContext commonDataContext) : base(commonDataContext)
        {
        }

        public async Task<List<Models.OrganisationModel>> GetOrganisationsAsync()
        {
            return await _commonDataContext.Organisations
                .OrderBy(x => x.Name)
                .Select(x => new Models.OrganisationModel(x)).ToListAsync();
        }


        public async Task<Models.OrganisationModel> GetOrganisationAsync(Guid id)
        {
            Data.Organisation dbOrganisation = await _commonDataContext.Organisations.FirstOrDefaultAsync(x => x.Id == id);

            return new Models.OrganisationModel(dbOrganisation);
        }

        public async Task<Models.OrganisationModel> UpdaterOrInsertAsync(Models.OrganisationModel model)
        {
            Data.Organisation dbOrganisation = await _commonDataContext.Organisations.FirstOrDefaultAsync(x => x.Id == model.Id);

            bool isNew = false;

            if (dbOrganisation == null)
            {
                isNew = true;
                dbOrganisation = new Organisation();
            }

            dbOrganisation.Id = model.Id;
            dbOrganisation.Name = model.Name;
            dbOrganisation.Number = (ushort)model.Number;

            if (isNew)
            {
                dbOrganisation.Created = DateTime.UtcNow;
                await _commonDataContext.AddAsync(dbOrganisation);
            }

            await _commonDataContext.SaveChangesAsync();

            return model;
        }
    }
}
