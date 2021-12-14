using Mechanics.InteractionSystem.Interactors;
using UnityEngine;

namespace Mechanics.InteractionSystem
{
    [RequireComponent(typeof(RayInteractor))]
    public class CursorDrawer : MonoBehaviour, ICursor
    {
        [SerializeField] private GameObject cursorPrefab;
        private GameObject cursor;
        private RayInteractor rayInteractor;

        private void OnEnable()
        {
            if (cursorPrefab == null)
            {
                enabled = false;
                return;
            }

            rayInteractor = GetComponent<RayInteractor>();
            rayInteractor.ProcessAction += GetPositionAndRotation;
            if (cursor == null)
            {
                cursor = Instantiate(cursorPrefab);
                cursor.transform.parent = transform;
            }
            cursor.SetActive(true);
        }

        private void OnDisable()
        {
            if (cursor != null)
            {
                cursor.SetActive(false);
            }
            rayInteractor.ProcessAction -= GetPositionAndRotation;
        }

        public void GetPositionAndRotation(RaycastHit raycastHit)
        {
            if (raycastHit.transform == null)
            {
                cursor.SetActive(false);
                return;
            }
            cursor.SetActive(true);
            var rotation = raycastHit.normal.sqrMagnitude > 0 ? Quaternion.LookRotation(raycastHit.normal) : Quaternion.identity;
            SetCursorPositionAndRotation(raycastHit.point, rotation);
        }

        public void SetCursorPositionAndRotation(Vector3 position, Quaternion rotation)
        {
            cursor.transform.position = position;
            cursor.transform.rotation = rotation;
        }
    }
}
