using System;
using Mechanics.Level;
using UnityEngine;

namespace Data.LevelSettings
{
    [Serializable]
    public class LayerSet 
    {
        public LayerMask LayerMask => layerMask;
        public KeyPointState KewKeyPointState => keyPointState;
        public int Cost
        {
            get => cost;
            set => cost = value;
        }

        [SerializeField] private string name;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private KeyPointState keyPointState;
        [SerializeField] private int cost;

        public override string ToString()
        {
            return name;
        }

        public static bool operator ==(LayerSet lhl, LayerMask rhl)
        {
            return (lhl.layerMask & rhl) != 0;
        }

        public static bool operator !=(LayerSet lhl, LayerMask rhl)
        {
            return !(lhl == rhl);
        }
        protected bool Equals(LayerSet other)
        {
            return name == other.name &&
                   layerMask.Equals(other.layerMask) &&
                   cost == other.cost &&
                   keyPointState.Equals(other.keyPointState);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LayerSet)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (name != null ? name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ layerMask.GetHashCode();
                hashCode = (hashCode * 397) ^ cost;
                hashCode = (hashCode * 397) ^ (int)keyPointState;
                return hashCode;
            }
        }

    }
}
