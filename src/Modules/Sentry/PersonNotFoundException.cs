namespace DigitalGuardBook.Modules.Sentry;

public sealed class PersonNotFoundException : InvalidOperationException
{
    public PersonNotFoundException(string personId)
        : base($"Person with ID '{personId}' not found.")
    {
        PersonId = personId;
    }

    public string PersonId { get; }
}
