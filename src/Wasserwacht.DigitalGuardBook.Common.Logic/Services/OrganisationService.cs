using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Data;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Services
{
    class OrganisationService : BaseService
    {
        public OrganisationService(CommonDataContext commonDataContext) : base(commonDataContext)
        {
        }

        public async Task<List<Models.OrganistationModel>> GetOrganisationsAsync()
        {
            return await _commonDataContext.Organisations
                .OrderBy(x => x.Name)
                .Select(x => new Models.OrganistationModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Number = x.Number
                }).ToListAsync();
        }


        public async Task<Models.OrganistationModel> GetOrganisationAsync(Guid id)
        {
            Data.Organisation dbOrganisation;

            if (!await _commonDataContext.Organisations.AnyAsync(x => x.Id == id))
            {
                dbOrganisation = null;
            }
            else
            {
                dbOrganisation = await _commonDataContext.Organisations.FirstOrDefaultAsync(x => x.Id == id);
            }

            return new Models.OrganistationModel
            {
                Id = dbOrganisation?.Id,
                Name = dbOrganisation?.Name,
                Number = dbOrganisation?.Number,
            };
        }
    }
}
