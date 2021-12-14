using System.Collections.Generic;
using Data.LevelSettings;
using Mechanics.Controllers;
using UnityEditor;
using UnityEngine;
using Utilities;

namespace Mechanics.Level
{
    [CreateAssetMenu(fileName = "LevelController", menuName = "Game Config/Controllers/Level Controller")]
    public class LevelController : BaseController
    {
        public Dictionary<Vector2, KeyPoint> LevelKeyPoints => keyLevelPoints;

        private LevelSetting levelSetting;

        private ILevel level;

        [SerializeField] 
        private int tileCountByX;

        [SerializeField, Range(.01f, 1.0f)] 
        private float roundRobinRadius = 0.1f;

        [SerializeField] 
        private bool debug;

        private readonly Dictionary<Vector2, KeyPoint> keyLevelPoints =
            new Dictionary<Vector2, KeyPoint>();

        public override void UpdateProcess() { }

        public override void LateUpdateProcess() { }

        public override void Debug()
        {
            if (!Application.isPlaying || !debug)
            {
                return;
            }

            foreach (var keyLevelPoint in keyLevelPoints)
            {
                Gizmos.color = keyLevelPoint.Value.KeyLevelPointState == KeyPointState.Free ?
                    Color.gray
                    : keyLevelPoint.Value.KeyLevelPointState == KeyPointState.Way ?
                        Color.blue :
                        keyLevelPoint.Value.KeyLevelPointState == KeyPointState.Station ?
                            Color.red :
                            keyLevelPoint.Value.KeyLevelPointState == KeyPointState.NotFree ?
                                Color.cyan : 
                                Color.green;
                Handles.color = Color.yellow;
                Handles.DrawWireDisc(keyLevelPoint.Value.Position, Vector3.up,
                    keyLevelPoint.Value.InteractionRadius);

                Gizmos.DrawSphere(keyLevelPoint.Value.Position, .25f);
            }
        }

        public void BuildGrid(ILevel level)
        {
            if (level.MeshGameObject == null || tileCountByX < 1)
            {
                return;
            }

            this.level = level;
            levelSetting = new LevelSetting(level.MeshGameObject, tileCountByX);

            GetLevelPoints();
        }

        private void GetLevelPoints()
        {
            for (var x = levelSetting.MinX; x < levelSetting.MaxX; x++)
            {
                for (var z = levelSetting.MinZ; z < levelSetting.MaxZ; z++)
                {
                    var mapLocation = GetKeyPointMapLocation(levelSetting.TileScale, x, z, out var worldPosition);

                    if (keyLevelPoints.ContainsKey(mapLocation))
                    {
                        var keyPoint = keyLevelPoints[mapLocation];
                        keyLevelPoints[mapLocation] = new KeyPoint(mapLocation, worldPosition, keyPoint.Cost,
                                                                   keyPoint.KeyLevelPointState, levelSetting.TileScale,
                                                                   keyPoint.InteractionRadius);
                    }
                    else
                    {
                        var cost = GetPointCostAndState(worldPosition, out var keyPointState);
                        keyLevelPoints.Add(mapLocation, new KeyPoint(mapLocation, worldPosition, cost, keyPointState));
                    }
                }
            }
        }

        public int GetPointCostAndState(Vector3 pointPosition, out KeyPointState keyPointState)
        {
            var resultCost = int.MinValue;
            keyPointState = KeyPointState.Free;
            var areaRadius = levelSetting.TileScale * roundRobinRadius / 2; 
            var raycastHits = Physics.SphereCastAll(pointPosition, areaRadius, Vector3.down);
            for (var i = 0; i < raycastHits.Length; i++)
            {
                var cost = Utility.GetCostAndKeyPointState(1 << raycastHits[i].collider.gameObject.layer, out var kps);
                if (cost > resultCost)
                {
                    keyPointState = kps;
                    resultCost = cost;
                }
            }
            return resultCost;
        }

        private Vector2 GetKeyPointMapLocation(float tileScale, int x, int z, out Vector3 worldPosition)
        {
            var X = level.MeshGameObject.transform.right *
                         (tileScale * x + (levelSetting.TileXCount % 2 == 0 ? tileScale / 2 : 0));
            var Y = level.MeshGameObject.transform.parent.up * level.MeshGameObject.transform.lossyScale.y * .5f;

            var Z = level.MeshGameObject.transform.forward *
                    (tileScale * z + (levelSetting.TileZCount % 2 == 0 ? tileScale / 2 : 0));

            worldPosition = level.MeshGameObject.transform.position + X + Y + Z;

            return new Vector2(x, z);
        }
    }
}
