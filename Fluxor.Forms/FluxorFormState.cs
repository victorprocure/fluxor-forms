using System;
using System.Collections.Generic;

namespace Fluxor.Forms
{
    public record FluxorFormState
    {
        public FluxorFormInitializationStatus InitializationStatus { get; init; }

        internal IReadOnlyDictionary<Guid, object> FormsModelStorage { get; init; } = new Dictionary<Guid, object>();

        public object? this[Guid formId]
        {
            get
            {
                try
                {
                    var formValues = FormsModelStorage[formId];
                    return formValues;
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
