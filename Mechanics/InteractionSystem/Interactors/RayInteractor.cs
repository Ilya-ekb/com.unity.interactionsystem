using System;
using System.Collections.Generic;
using System.Linq;
using Main;
using UnityEngine;
using UnityEngine.InputSystem;
using Mechanics.InteractionSystem.Interactables;

namespace Mechanics.InteractionSystem.Interactors
{
    public class RayInteractor : BaseInteractor
    {
        public override bool IsSelectionActive
        {
            get
            {
                if (!base.IsSelectionActive)
                {
                    return false;
                }

                return ClickAction.phase == InputActionPhase.Started;
            }
        }

        public RaycastHit NearestRayCast
        {
            get
            {
                if(raycastHitAll.Count > 0)
                {
                    return raycastHitAll[0];
                }
                
                return default;
            }
        }

        public Action<RaycastHit> ProcessAction { get; set; }

        [Space,Header("Input Actions: ")]
        public InputAction ClickAction;
        [Space, Header("Position Actions: ")]
        public InputAction PositionAction;
        [Space, Header("Reset Actions: ")]
        public InputAction ResetAction;

        private Vector2 mousePosition;
        private float maxRayDistance;
        private Camera rootCamera;
        private List<RaycastHit> raycastHitAll = new List<RaycastHit>();

        [SerializeField] private bool debug;

        private Comparison<IInteractable> interactableSortComparison;
        private readonly Dictionary<IInteractable, float> interactableDistanceMap =
            new Dictionary<IInteractable, float>();

        public override void Register()
        {
            base.Register();

            RegisterSettings();
        }
        public override void Unregister()
        {
            base.Unregister();

            DisableInputActions();
        }


        private void RegisterSettings()
        {
            if (SettingsContainer.Instance == null || InteractionManager == null)
            {
                enabled = false;
                return;
            }

            rootCamera = Camera.allCameras.FirstOrDefault();
            maxRayDistance = SettingsContainer.Instance.InputSettings.MaxRayCursorDistance;
            interactionLayerMask = SettingsContainer.Instance.InputSettings.InteractionLayerMask;
            raycastLayerMask = SettingsContainer.Instance.InputSettings.RaycastLayerMask;

            interactableSortComparison = InteractableSortComparison;

            EnableInputActions();
        }

        public override bool CanSelect(IInteractable interactable)
        {
            return base.CanSelect(interactable) && SelectObject == null;
        }

        public override void Process()
        {
            if (rootCamera == null)
            {
                return;
            }
            var ray = rootCamera.ScreenPointToRay(mousePosition);

            if (debug)
            {
                Debug.DrawRay(ray.origin, ray.direction * maxRayDistance, Color.green);
            }

            raycastHitAll = Physics.RaycastAll(ray, maxRayDistance, raycastLayerMask).ToList();
            raycastHitAll.Sort(RaycastSortComparison);

            InteractionTransform.position = GetAttachPosition(SelectObject);
            InteractionTransform.rotation = GetAttachRotation(SelectObject);

            ProcessAction?.Invoke(NearestRayCast);
        }

        public override void GetValidObjects(List<IInteractable> objects)
        {
            objects.Clear();
            interactableDistanceMap.Clear();

            foreach (var raycastHit in raycastHitAll)
            {
                var interactable = InteractionManager.GetInteractable(raycastHit.collider);
                if (interactable == null)
                {
                    break;
                }

                if (objects.Contains(interactable))
                {
                    continue;
                }

                objects.Add(interactable);
                interactableDistanceMap.Add(interactable, interactable.GetDistanceToInteractor(this));
            }

            objects.Sort(interactableSortComparison);
        }

        private void EnableInputActions()
        {
            ClickAction.Enable();
            ResetAction.Enable();
            PositionAction.Enable();
            PositionAction.performed += CursorPositionAction_performed;
        }

        private void DisableInputActions()
        {
            ClickAction.Disable();
            ResetAction.Disable();
            PositionAction.Disable();
            PositionAction.performed -= CursorPositionAction_performed;
        }
        private void CursorPositionAction_performed(InputAction.CallbackContext obj)
        {
            mousePosition = obj.action.ReadValue<Vector2>();
        }

        protected virtual Vector3 GetAttachPosition(IInteractable interactable)
        {
            return interactable == null ? transform.position : interactable.BindingComponent.transform.position;
        }
        protected virtual Quaternion GetAttachRotation(IInteractable interactable)
        {
            return interactable == null ? transform.rotation : interactable.BindingComponent.transform.rotation;
        }

        private int InteractableSortComparison(IInteractable a, IInteractable b)
        {
            var aDist = interactableDistanceMap[a];
            var bDist = interactableDistanceMap[b];
            if (aDist > bDist)
            {
                return 1;
            }

            if (bDist > aDist)
            {
                return -1;
            }

            return 0;
        }

        private int RaycastSortComparison(RaycastHit a, RaycastHit b)
        {
            if (raycastHitAll.Count <= 1)
            {
                return 0;
            }
            var aDist = a.distance;
            var bDist = b.distance;
            if (aDist > bDist)
            {
                return 1;
            }

            if (bDist > aDist)
            {
                return -1;
            }
            return 0;
        }
    }
}
