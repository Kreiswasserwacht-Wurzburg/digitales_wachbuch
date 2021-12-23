using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wasserwacht.DigitalGuardBook.Common.Data;

namespace Wasserwacht.DigitalGuardBook.Common.Logic.Models
{
    public class PersonLoginModel
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string MidName { get; set; }

        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public bool HasAccount { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        public PersonLoginModel()
        { }

        public PersonLoginModel(Person dbPerson)
        {
            this.Id = dbPerson.Id;
            this.FirstName = dbPerson.FirstName;
            this.MidName = dbPerson.MidName;
            this.LastName = dbPerson.LastName;
            this.Email = dbPerson.Email;
            this.HasAccount = !string.IsNullOrEmpty(dbPerson.PasswordHash);
            this.Password = dbPerson.PasswordHash;
        }
    }
}
