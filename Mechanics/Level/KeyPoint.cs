using UnityEngine;
using UnityEngine.UIElements;

namespace Mechanics.Level
{
    public readonly struct KeyPoint
    {
        public Vector2 MapLocation { get; }
        public Vector3 Position { get; }
        public KeyPointState KeyLevelPointState { get; }
        public Bounds Bounds { get; }
        public float InteractionRadius { get; }
        public int Cost { get; }

        public KeyPoint(Vector2 mapLocation, Vector3 position, int cost, KeyPointState keyLevelPointState = KeyPointState.Free, float boundsSize = 1.0f, float interactionRadius = 1.0f)
        {
            MapLocation = mapLocation;
            KeyLevelPointState = keyLevelPointState;
            Position = position;
            Cost = cost;
            Bounds = new Bounds(position, Vector3.one * boundsSize);
            InteractionRadius = interactionRadius;
        }

        public bool Equals(KeyPoint other)
        {
            return MapLocation.Equals(other.MapLocation) &&
                   Position.Equals(other.Position) &&
                   Bounds.Equals(other.Bounds) &&
                   InteractionRadius.Equals(other.InteractionRadius) &&
                   Cost == other.Cost;
        }

        public override bool Equals(object obj)
        {
            return obj is KeyPoint other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = MapLocation.GetHashCode();
                hashCode = (hashCode * 397) ^ Position.GetHashCode();
                hashCode = (hashCode * 397) ^ Bounds.GetHashCode();
                hashCode = (hashCode * 397) ^ InteractionRadius.GetHashCode();
                hashCode = (hashCode * 397) ^ Cost;
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"<Color=Orange>KeyPoint: [{MapLocation.x}:{MapLocation.y}]</Color> position: {Position}";
        }

        public static bool operator ==(KeyPoint aKeyPoint, KeyPoint bKeyPoint)
        {
            return aKeyPoint.Position == bKeyPoint.Position &&
                   aKeyPoint.MapLocation == bKeyPoint.MapLocation &&
                   aKeyPoint.Bounds == bKeyPoint.Bounds &&
                   Mathf.Approximately(aKeyPoint.InteractionRadius, bKeyPoint.InteractionRadius) &&
                   aKeyPoint.Cost == bKeyPoint.Cost;
        }

        public static bool operator !=(KeyPoint aKeyPoint, KeyPoint bKeyPoint)
        {
            return !(aKeyPoint == bKeyPoint);
        }
    }
}
