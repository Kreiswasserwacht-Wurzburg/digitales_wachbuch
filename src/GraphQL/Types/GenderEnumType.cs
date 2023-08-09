using DigitalGuardBook.Data.Entities;
using GraphQL.Types;

namespace DigitalGuardBook.GraphQL.Types
{
    public class GenderEnumType : EnumerationGraphType<Gender>
    {
        public GenderEnumType()
        {
            Name = "Gender";
            Description = "Gender of the person";
        }
    }
}