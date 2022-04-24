using System.Threading.Tasks;
using Fluxor.Forms.Actions;

namespace Fluxor.Forms
{
    internal class FluxorFormsMiddleware : Middleware
    {
        public override async Task InitializeAsync(IDispatcher dispatcher, IStore store)
        {
            dispatcher.Dispatch(new FluxorFormsInitialized(FluxorFormInitializationStatus.Succeeded));

            await base.InitializeAsync(dispatcher, store);
        }
    }
}