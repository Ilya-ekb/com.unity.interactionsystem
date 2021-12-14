using Mechanics.InteractionSystem.Interactables;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.InteractionSystem.Interactors
{
    public abstract class BaseInteractor : MonoBehaviour, IInteractor
    {
        public InteractionManager InteractionManager { get; private set; }

        public IInteractable SelectObject { get; protected set; }

        public LayerMask InteractionLayerMask => interactionLayerMask;
        public Action<SelectEventArgs> StartInteractionAction { get; set; }
        public Action<DeselectEventArgs> EndInteractionAction { get; set; }
        public Transform InteractionTransform => interactionTransform;

        public virtual bool IsSelectionActive => AllowSelect;

        public bool AllowSelect 
        {
            get => allowSelect; set => allowSelect = value; 
        }

        protected List<IInteractable> validObjects = new List<IInteractable>();
        protected LayerMask raycastLayerMask;
        protected LayerMask interactionLayerMask;
        [SerializeField] private bool allowSelect;
        [SerializeField] private Transform interactionTransform;

        private void OnEnable()
        {
            Register();
        }

        private void OnDisable()
        {
            Unregister();
        }

        public virtual void Register()
        {
            InteractionManager = InteractionManager.Instance;

            if (interactionTransform == null)
            {
                interactionTransform = new GameObject($"[Attach] {name}").transform;
                interactionTransform.parent = transform;
                interactionTransform.position = transform.position;
                interactionTransform.rotation = transform.rotation;
            }

            if (InteractionManager == null)
            {
                enabled = false;
                return;
            }
            InteractionManager?.RegisterInteractor(this);
        }

        public virtual void Unregister()
        {
            InteractionManager?.UnregisterInteractor(this);
        }
        public virtual bool CanSelect(IInteractable interactable)
        {
            return (interactionLayerMask & interactable.InteractionLayerMask) != 0;
        }

        public virtual void StartingInteraction(IInteractionEventArgs args)
        {
            SelectObject = args.Interactable;
        }

        public virtual void StartedInteraction(IInteractionEventArgs args)
        {
            StartInteractionAction?.Invoke((SelectEventArgs) args);
        }

        public virtual void EndingInteraction(IInteractionEventArgs args)
        {
            if (args.Interactable == SelectObject)
            {
                SelectObject = null;
            }
        }

        public virtual void EndedInteraction(IInteractionEventArgs args)
        {
            EndInteractionAction?.Invoke((DeselectEventArgs) args);
        }

        public abstract void Process();
        public abstract void GetValidObjects(List<IInteractable> objects);
    }
}
