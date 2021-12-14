using System;
using Mechanics.Controllers;
using Mechanics.InteractionSystem.Interactors;
using UnityEngine;

namespace Mechanics.InteractionSystem.Interactables
{
    public abstract class BaseInteractable<TComponent, TController> : MonoBehaviour, IInteractable
        where TComponent : Component
        where TController : BaseController
    {
        public InteractionManager InteractionControllerManager { get; private set; }

        public bool IsSelected { get; private set; }
        public Action<SelectEventArgs> SelectAction { get; set; }
        public Action<DeselectEventArgs> DeselectAction { get; set; }

        public Component BindingComponent
        {
            get
            {
                if (bindingComponent == null)
                {
                    bindingComponent = GetComponentInChildren<TComponent>();
                }

                return bindingComponent;
            }
        }

        public LayerMask InteractionLayerMask => 1 << gameObject.layer;

        protected TComponent bindingComponent;
        protected TController controller;

        protected virtual void OnEnable()
        {
            InteractionControllerManager = InteractionManager.Instance;

            Register();
        }

        protected virtual void OnDisable()
        {
            Unregister();
        }

        public virtual void Register()
        {
            InteractionControllerManager?.RegisterInteractable(this);
        }

        public virtual void Unregister()
        {
            InteractionControllerManager?.UnregisterInteractable(this);
        }

        public virtual void Selecting(IInteractionEventArgs args)
        {
            IsSelected = true;
        }

        public virtual void Selected(IInteractionEventArgs args)
        {
            SelectAction?.Invoke((SelectEventArgs)args);
        }

        public virtual void Deselecting(IInteractionEventArgs args)
        {
            IsSelected = false;
        }

        public virtual void Deselected(IInteractionEventArgs args)
        {
            DeselectAction?.Invoke((DeselectEventArgs)args);
        }

        public float GetDistanceToInteractor(IInteractor interactor)
        {
            if (interactor == null)
            {
                return float.MaxValue;
            }

            var minDistance = float.MaxValue;

            var currentDist = interactor.InteractionTransform.position - transform.position;
            return Math.Min(minDistance, currentDist.sqrMagnitude);
        }

        public abstract void Process();

        public virtual bool IsSelectableBy(IInteractor interactor)
        {
            return (interactor.InteractionLayerMask & InteractionLayerMask) != 0;
        }
    }
}
