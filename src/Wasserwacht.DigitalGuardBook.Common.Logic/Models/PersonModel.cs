using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Models
{
    public class PersonModel
    {
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
        public string FirstName { get; set; }

        public string MidName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        public Data.Gender Gender { get; set; }

        [Required, MinLength(1)]
        public List<OrganisationModel> Organisations { get; set; }

        public PersonModel()
        {
            Organisations = new List<OrganisationModel>();
        }

        public PersonModel(Data.Person dbPerson)
        {
            Id = dbPerson?.Id ?? Guid.NewGuid();
            FirstName = dbPerson?.FirstName;
            MidName = dbPerson?.MidName;
            LastName = dbPerson?.LastName;
            Gender = dbPerson?.Gender ?? Data.Gender.Unknown;
            Organisations = dbPerson?.Organisations.Select(x => new Models.OrganisationModel(x)).ToList() ?? new List<OrganisationModel>();
        }
    }
}
