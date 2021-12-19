using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class Person : IdentityUser<Guid>
    {
        public Person()
        {
            MangedOrganisations = new HashSet<Organisation>();
            Organisations = new HashSet<Organisation>();
        }

        [PersonalData]
        public string FirstName { get; set; }
        [PersonalData]
        public string MidName { get; set; }
        [PersonalData]
        public string LastName { get; set; }
        [PersonalData]
        public Gender Gender { get; set; }

        public virtual ICollection<Organisation> MangedOrganisations { get; set; }

        public virtual ICollection<Organisation> Organisations { get; set; }
    }
}
