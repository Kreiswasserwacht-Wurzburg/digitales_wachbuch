using System;
using System.Linq;
using Microsoft.AspNetCore.Components.Forms;


namespace Wasserwacht.DigitalGuardBook.Common
{
    public class BootstrapClassProvider : FieldCssClassProvider
    {
        public override string GetFieldCssClass(EditContext editContext, in FieldIdentifier fieldIdentifier)
        {
            var isValid = !editContext.GetValidationMessages(fieldIdentifier).Any();

            return isValid ? "is-valid" : "is-invalid";
        }
    }
}
