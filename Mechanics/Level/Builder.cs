using System;
using System.Collections.Generic;
using System.Linq;
using Data.BuilderSettings;
using Main;
using Mechanics.InteractionSystem.Interactors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mechanics.Level
{
    public abstract class Builder<TInteractor, TBuilderSettings> : BaseBuilder where TInteractor : BaseInteractor
        where TBuilderSettings : BuilderSetting
    {
        public override IBuilder BuilderDecorator => builderDecorator;
        public override Action<Object> BuildAction { get; set; }

        [SerializeField] protected TBuilderSettings builderSettings;
        [SerializeField] private BaseBuilder builderDecorator;

        protected override IInteractor baseInteractor => interactor;

        protected TInteractor interactor;
        protected Dictionary<Vector2, KeyPoint> keyPointsMap;

        public override void Initiate()
        {
            keyPointsMap = null;    
            builderDecorator?.Initiate();
        }

        public override void StartSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap)
        {
            this.interactor = (TInteractor) interactor;

            if (this.keyPointsMap == null)
            {
                this.keyPointsMap = keyPointsMap;
            }
        }

        public override bool CanBuild(List<KeyPoint> keyPoints)
        {
            return keyPoints != null && keyPoints.Count > 0;
        }

        public override void ResetSettings()
        {
            var levelController = ServiceContainer.Instance.GetController<LevelController>();
            if (keyPointsMap != null)
            {
                var mls = keyPointsMap.Keys.ToArray();
                foreach (var mapLocation in mls)
                {
                    var kp = keyPointsMap[mapLocation];
                    keyPointsMap[mapLocation] = new KeyPoint(kp.MapLocation, kp.Position,
                        levelController.GetPointCostAndState(kp.Position, out var keyPointState), keyPointState,
                        kp.Bounds.size.x, kp.InteractionRadius);
                }
            }

            BuilderDecorator?.ResetSettings();
        }
    }
}
