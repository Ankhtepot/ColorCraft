using System.Collections.Generic;
using System.Linq;
using Controllers;
using Extensions;
using UnityEngine;
using static Utilities.Vector3Directions;

namespace Components
{
    public class SurroundingElementInfo : MonoBehaviour
    {
#pragma warning disable 649
        private static TerrainSpawnerController terrainSpawner;
        private static List<Vector3Int> groundElementsPositions;
#pragma warning restore 649

        private void Start()
        {
            terrainSpawner = FindObjectOfType<TerrainSpawnerController>();
        }

        public static bool ElementBellowIsNotDetached(Vector3 transformPosition)
        {
            return terrainSpawner.GridMap.ContainsKey(transformPosition.ToVector2IntXZ()) 
                   || BuiltElementsStoreController.ContainsKey(transformPosition);
        }
        
        public static List<Vector3Int> GetSurroundingElementsPositions(Vector3Int center)
        {
            return GetSurroundingPositions(center)
                .Where(checkedPosition => BuiltElementsStoreController.GetKeys().Contains(checkedPosition))
                .ToList();
        }
        
        private static IEnumerable<Vector3Int> GetSurroundingPositions(Vector3Int center)
        {
            return AllDirections.Select(direction => direction + center).ToList();
        }

        public static bool IsAnyPositionAroundGround(Vector3Int center)
        {
            if (groundElementsPositions == null)
            {
                groundElementsPositions = terrainSpawner.GetGroundElementsPositions();
            }
            
            return GetSurroundingPositions(center)
                .Any(position => groundElementsPositions.Contains(position));
        }
    }
}