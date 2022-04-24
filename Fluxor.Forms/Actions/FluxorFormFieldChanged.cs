using System;

namespace Fluxor.Forms.Actions
{
    public record FluxorFormFieldChanged(Guid FormId, string FieldName, object FormValues);
}
