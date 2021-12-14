using System;
using UnityEngine;

namespace Data.LevelSettings
{
    [Serializable]
    public readonly struct LevelSetting
    {
        public Vector3 Position => position;
        public Quaternion Rotation => rotation;
        public Vector3 LevelScale => levelScale;
        public float TileScale => tileScale;
        public int MinX => minX;
        public int MaxX => maxX;
        public int MinZ => minZ;
        public int MaxZ => maxZ;
        public int TileZCount => tileCountByZ;
        public int TileXCount => tileCountByX;

        private readonly GameObject levelMeshGameObject;
        private readonly Vector3 position;
        private readonly Quaternion rotation;
        private readonly Vector3 levelScale;
        private readonly int tileCountByX;
        private readonly int tileCountByZ;
        private readonly float tileScale;
        private readonly int minX;
        private readonly int maxX;
        private readonly int minZ;
        private readonly int maxZ;

        public LevelSetting(GameObject levelMeshGO, int tileXCount)
        {
            levelMeshGameObject = levelMeshGO;
            position = levelMeshGameObject.transform.position;
            rotation = levelMeshGameObject.transform.rotation;
            levelScale = levelMeshGameObject.transform.lossyScale;
            tileCountByX = tileXCount;
            tileScale = levelMeshGO.transform.lossyScale.x / tileCountByX;
            tileCountByZ = (int)(levelMeshGO.transform.lossyScale.z / tileScale);
            maxX = tileCountByX % 2 == 0 ? tileCountByX / 2 : (tileCountByX + 1) / 2;
            maxZ = tileCountByZ % 2 == 0 ? tileCountByZ / 2 : (tileCountByZ + 1) / 2;
            minX = tileCountByX % 2 == 0 ? -maxX : -maxX + 1;
            minZ = tileCountByZ % 2 == 0 ? -maxZ : -maxZ + 1;
        }

    }
}
