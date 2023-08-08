namespace DigitalGuardBook.Data.Entities
{
    public class OrganisationComposed : Organisation
    {
        public IList<OrganisationComposed> SubOrganisations { get; set; } = new List<OrganisationComposed>();
        public OrganisationComposed? ParentOrganisation { get; set; }

        public IList<Person> TechnicalLeads { get; set; } = new List<Person>();

        public IList<Person> Members { get; set; } = new List<Person>();
    }
}