using Main;
using Mechanics.Controllers;
using Mechanics.InteractionSystem.Interactors;
using Mechanics.Level;
using UnityEngine;

namespace Mechanics.InteractionSystem.Interactables
{
    public class ConstructorInteractable : BaseInteractable<Collider, LevelController>
    {
        private IBuilder builder;

        protected override void OnEnable()
        {
            base.OnEnable();
            builder?.Initiate();
        }

        public override void Process()
        {
            if (IsSelected)
            {
                builder?.Build();
            }
        }

        public override void Selecting(IInteractionEventArgs args)
        {
            base.Selecting(args);

            controller ??= ServiceContainer.Instance.GetController<LevelController>();

            if (builder != null)
            {
                return;
            }
            builder = SettingsContainer.Instance.BuilderItemSettings
                .Find(e => (e.LayerMask & InteractionLayerMask) != 0)?.Builder;
            if (args.Interactor is RayInteractor rayInteractor)
            {
                rayInteractor.ResetAction.started += _ => builder?.ResetSettings();
            }
        }

        public override void Selected(IInteractionEventArgs args)
        {
            base.Selected(args);
            if (args.Interactor is RayInteractor rayInteractor &&
                !(controller != null))
            {
                builder?.StartSettings(rayInteractor, controller?.LevelKeyPoints);
            }
        }

        public override void Deselected(IInteractionEventArgs args)
        {
            base.Deselected(args);
            if (args.Interactor is RayInteractor rayInteractor && 
                !(controller is null))
            {
                builder?.StopSettings(rayInteractor, controller?.LevelKeyPoints);
            }
        }
    }

}
