using Fluxor.DependencyInjection;

namespace Fluxor.Forms
{
    public static class FluxorFormsExtensions
    {
        public static FluxorOptions AddFluxorForms(this FluxorOptions options)
        {
            options.AddMiddleware<FluxorFormsMiddleware>();
            return options;
        }
    }
}
