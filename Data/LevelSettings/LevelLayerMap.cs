using UnityEngine;

namespace Data.LevelSettings
{
    [CreateAssetMenu(fileName = "LevelLayerMap", menuName = "Game Config/Level Settings/Level Layer Map")]
    public class LevelLayerMap : ScriptableObject
    {
        public LayerSet[] LayerSets;
        private void OnValidate()
        {
            for (var i = 0; i < LayerSets?.Length; i++)
            {
                LayerSets[i].Cost = i + 1;
            }
        }
    }
}
