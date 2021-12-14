using System;
using System.Collections.Generic;
using Mechanics.Controllers;
using UnityEngine;

namespace Main
{
    public class ServiceContainer : Singleton<ServiceContainer>
    {
        public Action UpdateAction { get; set; }
        public Action LateUpdateAction { get; set; }
        public Action DebugAction { get; set; }

        [SerializeField] private List<BaseController> baseControllers = new List<BaseController>();

        #region UnityMethods

        public void Initiate()
        {
            baseControllers.ForEach(e => e.Initiate());
        }

        private void Update()
        {
            UpdateAction?.Invoke();
        }
        private void LateUpdate()
        {
            LateUpdateAction?.Invoke();
        }

        private void OnDrawGizmos()
        {
            DebugAction?.Invoke();
        }

        #endregion

        public T GetController<T>() where T: BaseController => (T)baseControllers.Find(e => e is T);

        
    }
}
