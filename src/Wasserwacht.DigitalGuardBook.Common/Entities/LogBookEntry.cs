using System;
using System.ComponentModel.DataAnnotations;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public class LogBookEntry
    {
        [Key]
        public Guid Id { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        [Required]
        [MaxLength(500)]
        public string Subject { get; set; }

        public string Description { get; set; }

        public Person Author { get; set; }
    }
}
