using System;
using System.ComponentModel.DataAnnotations;

namespace Wasserwacht.DigitalGuardBook.Common.Data
{
    public abstract class EntityBase
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Created { get; set; }

        public string Creator { get; set; }
    }
}
