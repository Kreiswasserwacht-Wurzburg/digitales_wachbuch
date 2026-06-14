namespace DigitalGuardBook.Modules.Sentry;

public sealed class OrganisationNotFoundException : InvalidOperationException
{
    public OrganisationNotFoundException(string organisationId)
        : base($"Organisation with ID '{organisationId}' not found.")
    {
        OrganisationId = organisationId;
    }

    public string OrganisationId { get; }
}
