using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Fluxor.Blazor.Web.Components;
using Fluxor.Forms.Actions;
using Fluxor.Forms.Utilities;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Fluxor.Forms
{
    public class FluxorForm<TFormModel> : FluxorComponent
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
        public TFormModel InitialValues { get => _initialValues ?? throw new InvalidOperationException($"{nameof(InitialValues)} cannot be null"); set => _initialValues = value; }

        [Parameter]
        public RenderFragment<TFormModel>? ChildContent { get; set; }

        [Parameter]
        public bool UpdateStateWhenInvalid { get; set; } = false;

        [Parameter]
        public Guid FormId { get; set; } = Guid.NewGuid();

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, object>? AdditionalAttributes { get; set; }


        private TFormModel? _internalMutableModel;
        private TFormModel? _initialValues;

        protected override void OnParametersSet()
        {
            if (InitialValues is null)
            {
                throw new InvalidOperationException($"{nameof(InitialValues)} cannot be null.");
            }

            if (_editContext?.Model is not TFormModel)
            {
                _internalMutableModel = InitialValues.DeepCopy();
                var immutableModel = InitialValues.DeepCopy();
                _editContext = new EditContext(_internalMutableModel);
                _editContext.OnFieldChanged += OnFormFieldChanged;

                Dispatcher.Dispatch(new FluxorFormCreated(FormId, immutableModel!));
            }
        }

        private void OnFormFieldChanged(object? sender, FieldChangedEventArgs e)
        {
            if (_editContext is null)
            {
                throw new InvalidOperationException($"{nameof(_editContext)} cannot be null");
            }
            if (_internalMutableModel is null)
            {
                throw new InvalidOperationException($"{nameof(_internalMutableModel)} cannot be null");
            }

            var isValid = _editContext.Validate();

            if ((!UpdateStateWhenInvalid && isValid) || UpdateStateWhenInvalid)
            {
                var immutableModel = _internalMutableModel.DeepCopy();
                Dispatcher.Dispatch(new FluxorFormFieldChanged(FormId, e.FieldIdentifier.FieldName, immutableModel!));
            }
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (_editContext is null)
            {
                throw new InvalidOperationException($"{nameof(_editContext)} cannot be null");
            }

            if (_internalMutableModel is null)
            {
                throw new InvalidOperationException($"{nameof(_internalMutableModel)} cannot be null");
            }

            builder.OpenRegion(_editContext.GetHashCode());

            builder.OpenElement(0, "form");
            builder.AddMultipleAttributes(1, AdditionalAttributes);
            builder.OpenComponent<CascadingValue<EditContext>>(2);
            builder.AddAttribute(3, "IsFixed", true);
            builder.AddAttribute(4, "Value", _editContext);
            builder.AddAttribute(5, "ChildContent", ChildContent?.Invoke(_internalMutableModel));
            builder.CloseComponent();
            builder.CloseElement();

            builder.CloseRegion();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Dispatcher.Dispatch(new FluxorFormDisposing(FormId));
            }
        }
    }
}