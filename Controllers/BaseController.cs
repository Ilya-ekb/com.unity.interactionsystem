using Main;
using UnityEngine;

namespace Mechanics.Controllers
{
    public abstract class BaseController : ScriptableObject
    {
        public virtual void Initiate()
        {
            ServiceContainer.Instance.DebugAction += Debug;
            ServiceContainer.Instance.UpdateAction += UpdateProcess;
            ServiceContainer.Instance.LateUpdateAction += LateUpdateProcess;
        }

        public abstract void UpdateProcess();
        public abstract void LateUpdateProcess();
        public abstract void Debug();
    }
}
