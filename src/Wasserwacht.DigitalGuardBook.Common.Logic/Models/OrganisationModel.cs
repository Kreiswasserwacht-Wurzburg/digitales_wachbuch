using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Models
{
    public class OrganisationModel
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(500)]
        public string Name { get; set; }

        [Required]
        [Range(1, ushort.MaxValue)]
        public int? Number { get; set; }
    }
}
