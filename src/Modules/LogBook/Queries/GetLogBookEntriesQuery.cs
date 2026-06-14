namespace DigitalGuardBook.Modules.LogBook.Queries;

public sealed record GetLogBookEntriesQuery(DateTimeOffset From, DateTimeOffset? To);
