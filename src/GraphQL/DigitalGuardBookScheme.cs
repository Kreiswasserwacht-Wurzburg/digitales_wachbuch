using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookScheme : Schema
    {
        public DigitalGuardBookScheme(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetRequiredService<DigitalGuardBookQuery>();
            Mutation = provider.GetRequiredService<DigitalGuardBookMutation>();
            //Subscription= provider.GetRequiredService<DigitalGuardBookSubscription>();
        }
    }
}