using Mechanics.InteractionSystem.Interactors;
using System;
using UnityEngine;

namespace Mechanics.InteractionSystem.Interactables
{
    public interface IInteractable
    {
        Component BindingComponent { get; }
        InteractionManager InteractionControllerManager { get; }
        LayerMask InteractionLayerMask { get; }
        bool IsSelected { get; }
        Action<SelectEventArgs> SelectAction { get; set; }
        Action<DeselectEventArgs> DeselectAction { get; set; }
        void Register();
        void Unregister();
        
        bool IsSelectableBy(IInteractor interactor);
        
        void Selecting(IInteractionEventArgs args);
        void Selected(IInteractionEventArgs args);
        void Process();
        void Deselecting(IInteractionEventArgs args);
        void Deselected(IInteractionEventArgs args);

        float GetDistanceToInteractor(IInteractor interactor);
    }
}
