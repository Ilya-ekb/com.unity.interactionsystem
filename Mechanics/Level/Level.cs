using Main;
using UnityEngine;

namespace Mechanics.Level
{
    public class Level : Singleton<Level>, ILevel
    {
        public GameObject MeshGameObject => meshGameObject;
        
        [SerializeField] private GameObject meshGameObject;   

        private void OnEnable()
        {
            ServiceContainer.Instance.Initiate();
            ServiceContainer.Instance.GetController<LevelController>()?.BuildGrid(this);
        }
    }                    
}
