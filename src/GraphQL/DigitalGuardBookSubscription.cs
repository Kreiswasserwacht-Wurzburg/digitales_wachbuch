using DigitalGuardBook.Data.Entities;
using DigitalGuardBook.GraphQL.Types;
using DigitalGuardBook.Repositories;
using GraphQL;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL
{
    public class DigitalGuardBookSubscription: ObjectGraphType
    {
        public DigitalGuardBookSubscription()
        {

        }
        public static IObservable<LogBookEntryType> NewLogBookEntry(LogBookRepository logBookRepository, string? date = null)
            => date == null ? logBookRepository.SubscribeAll() : logBookRepository.SubscribeFromDate(date);
    }
}