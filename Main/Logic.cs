using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Main
{
    public class Logic : Singleton<Logic>
    {
        
        public Action<float> OnLogicUpdate { get; set; }

        [SerializeField] private float updatePeriod = 1.0f;

        private float timer;
        private void LogicUpdate()
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = updatePeriod;
                OnLogicUpdate?.Invoke(updatePeriod);
            }
        }

        private void Update()
        {
            LogicUpdate();
        }
    }
}
