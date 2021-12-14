using System;
using Mechanics.InteractionSystem.Interactors;
using System.Collections.Generic;

using UnityEngine;
using Object = UnityEngine.Object;

namespace Mechanics.Level
{
    public interface IBuilder
    {
        IBuilder BuilderDecorator { get; }
        Action<Object> BuildAction { get; set; }
        void Initiate();
        void StartSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap);
        bool CanBuild(List<KeyPoint> keyPoints);
        void Build(List<KeyPoint> keyPoints = null);
        void StopSettings(IInteractor interactor, Dictionary<Vector2, KeyPoint> keyPointsMap);
        void ResetSettings();
    }
}
