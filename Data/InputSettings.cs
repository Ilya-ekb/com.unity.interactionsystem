using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "InputSettings", menuName = "Game Config/Input Settings")]
    public class InputSettings : ScriptableObject
    {
        public float MaxRayCursorDistance => maxRayCursorDistance;
        public LayerMask InteractionLayerMask => interactionLayerMask;
        public LayerMask RaycastLayerMask => raycastLayerMask;

        [SerializeField] private float maxRayCursorDistance;
        [SerializeField] private LayerMask interactionLayerMask;
        [SerializeField] private LayerMask raycastLayerMask;
    }
}
