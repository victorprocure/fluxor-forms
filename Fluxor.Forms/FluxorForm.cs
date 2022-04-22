using System;
using System.Collections.Generic;
using Fluxor.Blazor.Web.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace Fluxor.Forms
{
    public class FluxorForm : FluxorComponent
    {
        private EditContext? _editContext;
        private IDispatcher? _dispatcher;
        private IState<FluxorFormState>? _formState;

        [Inject]
        public IDispatcher Dispatcher { get => _dispatcher ?? throw new InvalidOperationException($"{nameof(Dispatcher)} cannot be null"); set => _dispatcher = value; }

        [Inject]
        public IState<FluxorFormState> FormState { get => _formState ?? throw new InvalidOperationException($"{nameof(FormState)} cannot be null"); set => _formState = value; }

#if NET5_0_OR_GREATER
        [NotNullIfNotNull(nameof(_editContext))]
#endif
        [Parameter]
        public object? InitialValues { get; set; }

        [Parameter]
        public RenderFragment<EditContext>? ChildContent { get; set; }

        [Parameter]
        public bool UpdateStateWhenInvalid { get; set; } = false;

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }


        protected override void OnParametersSet()
        {
            if (InitialValues is null)
            {
                throw new InvalidOperationException($"{nameof(InitialValues)} cannot be null.");
            }

            if (InitialValues is not null && InitialValues != _editContext?.Model)
            {
                _editContext = new EditContext(InitialValues);
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_editContext is null)
            {
                throw new InvalidOperationException($"{nameof(_editContext)} cannot be null");
            }

            builder.OpenRegion(_editContext.GetHashCode());

            builder.OpenElement(0, "form");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.OpenComponent<CascadingValue<EditContext>>(2);
            builder.AddAttribute(3, "IsFixed", true);
            builder.AddAttribute(4, "Value", _editContext);
            builder.AddAttribute(5, "ChildContent", ChildContent?.Invoke(_editContext));
            builder.CloseComponent();
            builder.CloseElement();

            builder.CloseRegion();
        }
    }
}