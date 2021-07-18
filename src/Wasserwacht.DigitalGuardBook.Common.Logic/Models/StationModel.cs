using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Models
{
    public class StationModel
    {
        public Guid? Id { get; set; }

        [Required]
        [MaxLength(250)]
        public string Name { get; set; }

        [Required]
        [MaxLength(500)]
        public string Street { get; set; }

        [Required]
        [MaxLength(20)]
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string City { get; set; }

        [Phone]
        public string PhoneNumber { get; set; }
    }
}
