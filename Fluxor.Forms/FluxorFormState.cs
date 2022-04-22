using System;

namespace Fluxor.Forms
{
    public record FluxorFormState
    {
        private string? _formName;

        public string FormName { get => _formName ?? throw new InvalidOperationException($"{nameof(FormName)} must be set"); init => _formName = value; }
    }
}
