namespace DigitalGuardBook.Data.Entities
{
    public class PersonComposed : Person
    {
        public List<OrganisationComposed> Organisations { get; set; } = new List<OrganisationComposed>();
    }
}