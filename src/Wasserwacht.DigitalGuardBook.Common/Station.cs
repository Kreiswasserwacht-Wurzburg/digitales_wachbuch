using System;
using System.ComponentModel.DataAnnotations;

namespace Wasserwacht.DigitalGuardBook.Common
{
    public class Station
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
        [MaxLength(500)]
        public string Street { get; set; }
        [MaxLength(20)]
        public string ZipCode { get; set; }
        [MaxLength(100)]
        public string City { get; set; }
        [Phone]
        public string PhoneNumber { get; set; }
    }
}
