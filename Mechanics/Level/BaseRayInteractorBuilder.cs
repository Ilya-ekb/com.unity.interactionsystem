using System;
using System.Collections.Generic;
using Data.BuilderSettings;
using Mechanics.InteractionSystem.Interactors;
using UnityEngine;
using UnityEngine.Events;

namespace Mechanics.Level
{
    public abstract class BaseRayInteractorBuilder<TSetting> : Builder<RayInteractor, TSetting> where TSetting:BuilderSetting
    {
        public override void StartSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap)
        {
            base.StartSettings(interactor, keyPointsMap);

            if (this.builderSettings == null || this.interactor == null || this.keyPointsMap == null)
            {
                return;
            }

            base.interactor.ClickAction.canceled -= ClickAction_canceled;
            base.interactor.ClickAction.canceled += ClickAction_canceled;

            BuilderDecorator?.StartSettings(interactor, keyPointsMap);
        }

        public override void StopSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap)
        {
            BuilderDecorator?.StopSettings(interactor, keyPointsMap);
        }

        protected virtual void ClickAction_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
        {
            base.interactor.ClickAction.canceled -= ClickAction_canceled;
        }
    }
}
