using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.Repositories;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class SentryType : ObjectGraphType<Sentry>
    {
        public SentryType(OrganisationRepository organisationRepository)
        {
            Field(x => x.Id);
            Field(x => x.Start);
            Field(x => x.End, nullable: true);
            Field(x => x.Registration, nullable: true);
            Field<OrganisationType>("organisation")
            .ResolveAsync(async context => await organisationRepository.OrganisationAsync(context.Source.OrganisationId));
            Field<ListGraphType<GuardServiceType>>("supervisors").Resolve(context => context.Source.SupervisorServices);
            Field<ListGraphType<GuardServiceType>>("guards").Resolve(context => context.Source.GuardServices);
        }
    }
}