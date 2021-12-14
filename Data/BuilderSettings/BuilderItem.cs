using System;
using Mechanics.Level;
using UnityEngine;

namespace Data
{
    [Serializable]
    public class BuilderItem
    {
        public LayerMask LayerMask => layerMask;
        public IBuilder Builder =>  builder;

        [SerializeField] private LayerMask layerMask;
        [SerializeField] private BaseBuilder builder;
    }
}
