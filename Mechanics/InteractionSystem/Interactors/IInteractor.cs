using Mechanics.InteractionSystem.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.InteractionSystem.Interactors
{
    public interface IInteractor
    {
        InteractionManager InteractionManager { get; }
        IInteractable SelectObject { get; }
        bool IsSelectionActive { get; }
        bool AllowSelect { get; set; }
        LayerMask InteractionLayerMask { get; }
        Action<SelectEventArgs> StartInteractionAction { get; set; }
        Action<DeselectEventArgs> EndInteractionAction { get; set; }
        Transform InteractionTransform { get; }
        void Register();
        void Unregister();
        bool CanSelect(IInteractable interactable);
        void GetValidObjects(List<IInteractable> objects);
        void StartingInteraction(IInteractionEventArgs args);
        void StartedInteraction(IInteractionEventArgs args);
        void EndingInteraction(IInteractionEventArgs args);
        void EndedInteraction(IInteractionEventArgs args);
        void Process();
    }
}
