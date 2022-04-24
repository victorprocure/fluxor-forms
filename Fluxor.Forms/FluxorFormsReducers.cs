using System;
using System.Collections.Generic;
using System.Linq;
using Fluxor.Forms.Actions;

namespace Fluxor.Forms
{
    internal static class FluxorFormsReducers
    {
        [ReducerMethod]
        public static FluxorFormState ReduceFluxorFormsInitializedAction(FluxorFormState currentState, FluxorFormsInitialized action)
            => currentState with { InitializationStatus = action.InitializationStatus };

        [ReducerMethod]
        public static FluxorFormState ReduceFluxorFormCreated(FluxorFormState currentState, FluxorFormCreated action)
        {
            var newFormsDictionary = new Dictionary<Guid, object>(currentState.FormsModelStorage.ToDictionary(k => k.Key, v => v.Value))
            {
                { action.FormId, action.FormModel }
            };

            return currentState with { FormsModelStorage = newFormsDictionary };
        }

        [ReducerMethod]
        public static FluxorFormState ReduceFluxorFormFieldChanged(FluxorFormState currentState, FluxorFormFieldChanged action)
        {
            var updatableFormsDictionary = currentState.FormsModelStorage.ToDictionary(k => k.Key, v => v.Value);
            updatableFormsDictionary[action.FormId] = action.FormValues;

            return currentState with { FormsModelStorage = updatableFormsDictionary };
        }

        [ReducerMethod]
        public static FluxorFormState ReduceFluxorFormDisposing(FluxorFormState currentState, FluxorFormDisposing action)
        {
            var updatableFormsDictionary = currentState.FormsModelStorage.ToDictionary(k => k.Key, v => v.Value);
            if (!updatableFormsDictionary.ContainsKey(action.FormId))
            {
                return currentState;
            }

            updatableFormsDictionary.Remove(action.FormId);
            return currentState with { FormsModelStorage = updatableFormsDictionary };
        }
    }
}
