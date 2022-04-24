namespace Fluxor.Forms
{
    public class FluxorFormFeature : Feature<FluxorFormState>
    {
        public override string GetName() => "FluxorFormFeature";
        protected override FluxorFormState GetInitialState() =>
            new() { InitializationStatus = FluxorFormInitializationStatus.Initializing };
    }
}