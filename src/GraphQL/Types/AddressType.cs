using DigitalGuardBook.Data.Entities;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class AddressType : ObjectGraphType<Address>
    {
        public AddressType()
        {
            Field(x => x.Street);
            Field(x => x.ZipCode);
            Field(x => x.City);
        }
    }
}