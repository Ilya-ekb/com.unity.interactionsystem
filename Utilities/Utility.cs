using Main;
using Mechanics.Level;
using UnityEngine;

namespace Utilities
{
    public static class Utility
    {
        public static int GetCostAndKeyPointState(LayerMask layerMask, out KeyPointState keyPointState)
        {
            var layerSets = SettingsContainer.Instance.LevelLayerMap?.LayerSets;
            keyPointState = KeyPointState.Free;
            if(layerSets == null)
            {
                return int.MaxValue;
            }
            foreach (var layerSet in layerSets)
            {
                if (layerSet != layerMask)
                {
                    continue;
                }
                keyPointState = layerSet.KewKeyPointState;
                return layerSet.Cost;
            }
            return int.MaxValue;
        }
    }
}
