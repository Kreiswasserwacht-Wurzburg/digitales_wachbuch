using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class Organisation : EntityBase
    {
        public Organisation()
        {
            TechnicalLead = new HashSet<Person>();
            Stations = new HashSet<Station>();
            Members = new HashSet<Person>();
        }

        [Required]
        [MaxLength(500)]
        public String Name { get; set; }
        [Required]
        public ushort Number { get; set; }

        public virtual ICollection<Station> Stations { get; set; }

        public virtual ICollection<Person> TechnicalLead { get; set; }

        public virtual ICollection<Person> Members { get; set; }
    }
}
