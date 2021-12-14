using System;
using System.Collections.Generic;
using Mechanics.InteractionSystem.Interactors;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Mechanics.Level
{
    public abstract class BaseBuilder : ScriptableObject, IBuilder
    {
        public abstract IBuilder BuilderDecorator { get; }
        public abstract Action<Object> BuildAction { get; set; }
        protected abstract IInteractor baseInteractor { get; }
        public abstract void Initiate();
        public abstract void StartSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap);
        public abstract bool CanBuild(List<KeyPoint> keyPoints);
        public abstract void Build(List<KeyPoint> keyPoints = null);
        public abstract void StopSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap);
        public abstract void ResetSettings();
    }
}
