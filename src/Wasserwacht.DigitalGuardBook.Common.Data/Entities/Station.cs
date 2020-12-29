﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class Station : EntityBase
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

        public Organisation Organisation { get; set; }

        public virtual ICollection<LogBook> LogBooks { get; set; }
    }
}