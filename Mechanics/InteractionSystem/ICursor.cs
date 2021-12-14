using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics.InteractionSystem
{
    public interface ICursor
    {
        void SetCursorPositionAndRotation(Vector3 position, Quaternion rotation);
    }
}
