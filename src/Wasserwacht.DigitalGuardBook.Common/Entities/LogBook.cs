using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class LogBook : EntityBase
    {
        [Range(2020, 2035)]
        public int Year { get; set; }

        public Station Station { get; set; }

        public virtual ICollection<LogBookEntry> Entries { get; set; }
    }
}
