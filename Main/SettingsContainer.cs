using System.Collections.Generic;
using Data;
using Data.LevelSettings;
using UnityEngine;

namespace Main
{
    public class SettingsContainer : Singleton<SettingsContainer>
    {
        public GameDevSettings GameDevSettings => gameDevSettings;
        public InputSettings InputSettings => inputSettings;
        public LevelLayerMap LevelLayerMap => levelLayerMap;
        public List<BuilderItem> BuilderItemSettings => builderItemSettings;
        
        [SerializeField] private GameDevSettings gameDevSettings;
        [SerializeField] private InputSettings inputSettings;
        [SerializeField] private LevelLayerMap levelLayerMap;
        [SerializeField] private List<BuilderItem> builderItemSettings;
    }
}
