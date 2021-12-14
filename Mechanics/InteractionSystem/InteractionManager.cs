using System.Collections.Generic;
using Main;
using Mechanics.InteractionSystem.Interactables;
using Mechanics.InteractionSystem.Interactors;

using UnityEngine;

namespace Mechanics.InteractionSystem
{
    public class InteractionManager : Singleton<InteractionManager>
    {
        private readonly List<IInteractor> interactors = new List<IInteractor>();

        private readonly Dictionary<Component, IInteractable>
            interactables = new Dictionary<Component, IInteractable>();

        private readonly List<IInteractable> validInteractableObjects = new List<IInteractable>();
        private SelectEventArgs selectEventArgs;
        private DeselectEventArgs deselectEventArgs;

        #region Unity methods

        private void Update()
        {
            ProcessInteractor();

            foreach (var interactor in interactors)
            {
                GetValidInteractableObjects(interactor, validInteractableObjects);
                ClearSelection(interactor);
                SelectValidTarget(interactor, validInteractableObjects);
            }

            ProcessInteraction();
        }

        #endregion

        #region Interactor/Interactable registration/unregistration methods

        public void RegisterInteractor(IInteractor interactorController)
        {
            if (interactors.Contains(interactorController))
            {
                return;
            }

            interactors.Add(interactorController);
        }

        public void UnregisterInteractor(IInteractor interactor)
        {
            if (interactors.Contains(interactor))
            {
                interactors.Remove(interactor);
            }
        }

        public void RegisterInteractable(IInteractable interactable)
        {
            if (interactables.ContainsKey(interactable.BindingComponent))
            {
                return;
            }

            interactables.Add(interactable.BindingComponent, interactable);
        }

        public void UnregisterInteractable(IInteractable interactable)
        {
            if (interactables.ContainsKey(interactable.BindingComponent))
            {
                interactables.Remove(interactable.BindingComponent);
            }
        }

        #endregion

        #region Interaction methods
        public IInteractable GetInteractable(Component component)
        {
            if (!interactables.ContainsKey(component))
            {
                return null;
            }

            return interactables[component];
        }

        public void GetValidInteractableObjects(IInteractor interactor, List<IInteractable> validInteractableObjects)
        {
            interactor.GetValidObjects(validInteractableObjects);
            foreach (var interactable in validInteractableObjects)
            {
                if (interactables.ContainsKey(interactable.BindingComponent))
                {
                    continue;
                }

                validInteractableObjects.Remove(interactable);
            }
        }

        private void ClearSelection(IInteractor interactor)
        {
            if (interactor.SelectObject != null && (!interactor.IsSelectionActive ||
                                                    !interactor.CanSelect(interactor.SelectObject) ||
                                                    !interactor.SelectObject.IsSelectableBy(interactor)))
            {
                SelectExit(interactor, interactor.SelectObject);
            }
        }

        private void SelectValidTarget(IInteractor interactor, List<IInteractable> validObjects)
        {
            //if (!interactor.IsSelectionActive)
            //{
            //    return;
            //}

            for (var i = 0; i < validObjects.Count && interactor.IsSelectionActive; i++)
            {
                if (interactor.CanSelect(validObjects[i]) && validObjects[i].IsSelectableBy(interactor) &&
                    interactor.SelectObject != validObjects[i])
                {
                    SelectEnter(interactor, validObjects[i]);
                }
            }
        }

        private void ProcessInteractor()
        {
            foreach (var interactorController in interactors)
            {
                interactorController.Process();
            }
        }

        private void ProcessInteraction()
        {
            foreach (var interactable in interactables.Values)
            {
                interactable.Process();
            }
        }

        private void SelectEnter(IInteractor interactor, IInteractable interactable)
        {
            selectEventArgs = new SelectEventArgs(interactor, interactable);
            SelectEnter(interactor, interactable, selectEventArgs);
        }

        protected virtual void SelectEnter(IInteractor interactor, IInteractable interactable,
            SelectEventArgs selectEventArgs)
        {
            if (interactable != selectEventArgs.Interactable || interactor != selectEventArgs.Interactor)
            {
                Debug.LogWarning(
                    $"SelectEnter for {interactor} not equal {selectEventArgs.Interactor} or {interactable} not equal {selectEventArgs.Interactable}");
                return;
            }

            interactor.StartingInteraction(selectEventArgs);
            interactable.Selecting(selectEventArgs);

            interactor.StartedInteraction(selectEventArgs);
            interactable.Selected(selectEventArgs);
        }

        private void SelectExit(IInteractor interactor, IInteractable interactable)
        {
            deselectEventArgs = new DeselectEventArgs(interactor, interactable);
            SelectExit(interactor, interactable, deselectEventArgs);
        }

        protected virtual void SelectExit(IInteractor interactor, IInteractable interactable,
            DeselectEventArgs deselectEventArgs)
        {
            if (interactable != deselectEventArgs.Interactable || interactor != deselectEventArgs.Interactor)
            {
                Debug.LogWarning(
                    $"SelectExit for {interactor} not equal {deselectEventArgs.Interactor} or {interactable} not equal {deselectEventArgs.Interactable}");
                return;
            }

            interactor.EndingInteraction(deselectEventArgs);
            interactable.Deselecting(deselectEventArgs);

            interactor.EndedInteraction(deselectEventArgs);
            interactable.Deselected(deselectEventArgs);
        }
        #endregion
    }
}
